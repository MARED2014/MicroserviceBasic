# PizzaPulse

A basic .NET microservices sample: order a pizza, cook it, deliver it.

[English](#english) · [Türkçe](#türkçe)

---

## English

### What this repo is

PizzaPulse is a **learning** project, not a production system. It shows three services talking to each other with messages instead of calling each other over HTTP.

| Piece | Role |
|--------|------|
| **WebUI** | Customer screen: menu, cart, place order, watch status |
| **Ordering** | Catalog, cart, order record, order status |
| **Kitchen** | Cook queue (waiting / in oven / ready) |
| **Delivery** | Couriers and the trip to the customer |
| **BuildingBlocks** | Shared event types (`OrderPlaced`, `OrderBaked`, …) |

Services do **not** share database tables or entity classes. They only share the event contracts.

### Technologies

- .NET 10, ASP.NET Core
- MediatR (commands and queries **inside** one service)
- MassTransit + RabbitMQ (events **between** services)
- SQL Server + EF Core (Ordering, Delivery)
- Redis (cart, courier busy flag)
- MongoDB (kitchen tasks)
- Swagger
- Docker Compose (local SQL, Redis, Mongo, RabbitMQ)

### Solution layout

```text
PizzaPulse.BuildingBlocks     shared events
PizzaPulse.Ordering.*         Api / Application / Core / Infrastructure
PizzaPulse.Kitchen.*          Api / Application / Core / Infrastructure
PizzaPulse.Delivery.*         Api / Application / Core / Infrastructure
PizzaPulse.WebUI              MVC UI (HTTP client → Ordering only)
```

Each service: **Api** receives HTTP, **Application** holds MediatR handlers and MassTransit consumers, **Core** holds entities and repository interfaces, **Infrastructure** talks to SQL / Redis / Mongo.

### How a request works inside one service (MediatR)

Example: placing an order.

1. Browser submits the cart form on WebUI (`CartController.PlaceOrder`).
2. WebUI does **not** use MediatR. It HTTP POSTs to Ordering: `/api/orders`.
3. `OrdersController` in Ordering injects `IMediator` and runs:

   `_mediator.Send(new PlaceOrderCommand(...))`

4. MediatR finds `PlaceOrderHandler` in the Ordering Application project and calls `Handle`.
5. The handler reads the Redis cart, writes `Order` + `OrderItem` to SQL, clears the cart, then publishes an event (next section).
6. The controller returns `{ orderId }`. WebUI opens the order status page.

The same pattern is used everywhere else in the APIs: controller → `Send(command/query)` → handler → repository.

### How services talk to each other (MassTransit)

After the order is saved, `PlaceOrderHandler` calls:

`_publishEndpoint.Publish(new OrderPlaced(...))`

MassTransit puts that message on **RabbitMQ**. Kitchen is already listening with `OrderPlacedConsumer`. When the message arrives:

1. `OrderPlacedConsumer.Consume` runs.
2. It does **not** write Mongo itself. It runs MediatR again:

   `_mediator.Send(new CreateKitchenTaskCommand(...))`

3. `CreateKitchenTaskHandler` inserts a `KitchenTask` (status Waiting).

So: **MediatR stays inside the service. MassTransit carries the event across services.**

Kitchen and Delivery never call Ordering over HTTP. Ordering updates its own status when it **consumes** later events (`OrderPreparationStarted`, `OrderBaked`, `OrderOnTheWay`, `OrderDelivered`).

Queue names are prefixed per service (`ordering-order-baked`, `delivery-order-baked`) so both Ordering and Delivery receive `OrderBaked`.

### End-to-end story

| Step | Who | What happens | Order status |
|------|-----|----------------|--------------|
| 1 | You (Swagger or later the UI) | Create menu items `POST /api/menu` | — |
| 2 | You (Delivery Swagger) | Create a courier `POST /api/couriers` | — |
| 3 | You (WebUI) | Pick pizzas, add to cart (Redis) | — |
| 4 | You (WebUI) | Place order | **Received** |
| 5 | System | `OrderPlaced` → Kitchen task | Received |
| 6 | You (Kitchen Swagger) | `POST .../start-oven` | **Preparing** |
| 7 | You (Kitchen Swagger) | `POST .../ready` | **Baked** + courier assigned |
| 8 | You (Delivery Swagger) | `POST .../pickup` | **OnTheWay** |
| 9 | You (Delivery Swagger) | `POST .../complete` | **Delivered** |

WebUI covers steps 3–5 and status refresh. Kitchen and courier actions are operator steps; they live in Swagger on purpose in this basic sample.

### How to run locally

**Need:** [.NET 10 SDK](https://dotnet.microsoft.com/download), [Docker Desktop](https://www.docker.com/products/docker-desktop/).

```bash
docker compose up -d
```

Starts SQL Server `:1433`, Redis `:6379`, MongoDB `:27017`, RabbitMQ `:5672` (management UI http://localhost:15672 — `admin` / `Password123!`).

Passwords match `appsettings.json`. **Local demo only.**

Four terminals:

```bash
dotnet run --project PizzaPulse.Ordering.Api --launch-profile http
dotnet run --project PizzaPulse.Kitchen.Api --launch-profile http
dotnet run --project PizzaPulse.Delivery.Api --launch-profile http
dotnet run --project PizzaPulse.WebUI --launch-profile http
```

Or open `MicroserviceBasic.slnx` and start all four projects.

| App | URL |
|-----|-----|
| Ordering Swagger | http://localhost:5107/swagger |
| Kitchen Swagger | http://localhost:5041/swagger |
| Delivery Swagger | http://localhost:5045/swagger |
| WebUI | http://localhost:5120 |

SQL schemas are created with `EnsureCreated` on startup (no EF migrations).

### Try it

Create at least one pizza and one courier in Swagger **before** placing an order from the UI.

**Menu** — `POST http://localhost:5107/api/menu`

```json
{
  "name": "Margherita",
  "description": "Tomato, mozzarella, basil",
  "basePrice": 220,
  "isAvailable": true
}
```

Copy `id` from the response. Repeat for a second pizza if you want.

**Courier** — `POST http://localhost:5045/api/couriers`

```json
{
  "fullName": "Alex Rider",
  "phone": "05551112233",
  "vehiclePlate": "34 ABC 123",
  "isActive": true
}
```

**WebUI** — http://localhost:5120  
Enter a customer id, select pizzas, add to cart, fill name + address, click **Place order**. Open the status page.

**Kitchen** — replace `{orderId}` with the id from the UI:

- `POST http://localhost:5041/api/kitchen/tasks/{orderId}/start-oven`
- `POST http://localhost:5041/api/kitchen/tasks/{orderId}/ready`

Refresh the WebUI status page.

**Delivery**

- `GET http://localhost:5045/api/deliveries/{orderId}`
- `POST http://localhost:5045/api/deliveries/{orderId}/pickup`
- `POST http://localhost:5045/api/deliveries/{orderId}/complete`

If Kitchen or RabbitMQ is down, the order is still saved in Ordering but the kitchen task never appears.

### Out of scope (on purpose)

No auth, no automated tests, no outbox, no EF migrations, no kitchen/courier screens in WebUI.

---

## Türkçe

### Bu repo nedir?

PizzaPulse **öğrenme** projesidir, canlı sistem değildir. Üç servisin birbirini HTTP ile çağırmadan, mesajla konuşmasını gösterir.

| Parça | Görevi |
|--------|--------|
| **WebUI** | Müşteri: menü, sepet, sipariş ver, durumu izle |
| **Ordering** | Katalog, sepet, sipariş kaydı, sipariş durumu |
| **Kitchen** | Pişirme kuyruğu (bekliyor / fırında / hazır) |
| **Delivery** | Kuryeler ve teslimat |
| **BuildingBlocks** | Ortak event tipleri (`OrderPlaced`, `OrderBaked`, …) |

Servisler tablo veya entity paylaşmaz. Paylaşılan tek şey event sözleşmeleridir.

### Teknolojiler

- .NET 10, ASP.NET Core
- MediatR (komut/sorgu **aynı servisin içinde**)
- MassTransit + RabbitMQ (event **servisler arasında**)
- SQL Server + EF Core (Ordering, Delivery)
- Redis (sepet, kurye meşgul bilgisi)
- MongoDB (mutfak işi)
- Swagger
- Docker Compose (yerel SQL, Redis, Mongo, RabbitMQ)

### Klasörler

```text
PizzaPulse.BuildingBlocks     ortak event’ler
PizzaPulse.Ordering.*         Api / Application / Core / Infrastructure
PizzaPulse.Kitchen.*          Api / Application / Core / Infrastructure
PizzaPulse.Delivery.*         Api / Application / Core / Infrastructure
PizzaPulse.WebUI              MVC (yalnızca Ordering’e HTTP)
```

**Api** HTTP alır, **Application** MediatR handler ve MassTransit consumer tutar, **Core** entity ve repository arayüzü, **Infrastructure** SQL / Redis / Mongo.

### Bir servisin içinde istek nasıl yürür (MediatR)

Sipariş örneği:

1. WebUI sepet formunu gönderir (`CartController.PlaceOrder`).
2. WebUI MediatR **kullanmaz**. Ordering’e HTTP `POST /api/orders` atar.
3. Ordering’deki `OrdersController` `IMediator` alır ve şunu çalıştırır:

   `_mediator.Send(new PlaceOrderCommand(...))`

4. MediatR, Application projesindeki `PlaceOrderHandler`’ı bulur, `Handle`’ı çağırır.
5. Handler Redis sepetini okur, SQL’e `Order` + `OrderItem` yazar, sepeti siler, sonra event basar (bir sonraki bölüm).
6. Controller `{ orderId }` döner. WebUI sipariş durum sayfasını açar.

API’lerdeki diğer işlemler de aynıdır: controller → `Send` → handler → repository.

### Servisler birbirine nasıl haber verir (MassTransit)

Sipariş kaydından sonra `PlaceOrderHandler` şunu çağırır:

`_publishEndpoint.Publish(new OrderPlaced(...))`

MassTransit mesajı **RabbitMQ**’ya koyar. Kitchen `OrderPlacedConsumer` ile dinlemektedir. Mesaj gelince:

1. `OrderPlacedConsumer.Consume` çalışır.
2. Mongo’ya kendisi yazmaz. Tekrar MediatR kullanır:

   `_mediator.Send(new CreateKitchenTaskCommand(...))`

3. `CreateKitchenTaskHandler` `KitchenTask` ekler (Waiting).

**MediatR servisin içindedir. MassTransit event’i servisten servise taşır.**

Kitchen ve Delivery, Ordering’i HTTP ile aramaz. Ordering kendi status’ünü sonradan gelen event’leri **tüketerek** günceller (`OrderPreparationStarted`, `OrderBaked`, `OrderOnTheWay`, `OrderDelivered`).

Kuyruk adları servise göre öneklenir (`ordering-order-baked`, `delivery-order-baked`); `OrderBaked` hem Ordering’e hem Delivery’ye düşer.

### Uçtan uca akış

| Adım | Kim | Ne olur | Sipariş durumu |
|------|-----|---------|----------------|
| 1 | Sen (Swagger) | Menü kaydı `POST /api/menu` | — |
| 2 | Sen (Delivery Swagger) | Kurye `POST /api/couriers` | — |
| 3 | Sen (WebUI) | Pizza seç, sepete ekle (Redis) | — |
| 4 | Sen (WebUI) | Sipariş ver | **Alındı** |
| 5 | Sistem | `OrderPlaced` → mutfak işi | Alındı |
| 6 | Sen (Kitchen Swagger) | `POST .../start-oven` | **Hazırlanıyor** |
| 7 | Sen (Kitchen Swagger) | `POST .../ready` | **Pişti** + kurye atanır |
| 8 | Sen (Delivery Swagger) | `POST .../pickup` | **Yolda** |
| 9 | Sen (Delivery Swagger) | `POST .../complete` | **Teslim edildi** |

WebUI 3–5. adımlar ve durum yenileme. Fırın ve kurye bu basic örnekte operatör işidir; Swagger’dadır.

### Yerelde çalıştırma

**Gerekli:** [.NET 10 SDK](https://dotnet.microsoft.com/download), [Docker Desktop](https://www.docker.com/products/docker-desktop/).

```bash
docker compose up -d
```

SQL Server `:1433`, Redis `:6379`, MongoDB `:27017`, RabbitMQ `:5672` (panel http://localhost:15672 — `admin` / `Password123!`).

Şifreler `appsettings.json` ile aynı. **Sadece local demo.**

Dört terminal:

```bash
dotnet run --project PizzaPulse.Ordering.Api --launch-profile http
dotnet run --project PizzaPulse.Kitchen.Api --launch-profile http
dotnet run --project PizzaPulse.Delivery.Api --launch-profile http
dotnet run --project PizzaPulse.WebUI --launch-profile http
```

Ya da `MicroserviceBasic.slnx` içinde dört projeyi birden başlatın.

| Uygulama | Adres |
|----------|--------|
| Ordering Swagger | http://localhost:5107/swagger |
| Kitchen Swagger | http://localhost:5041/swagger |
| Delivery Swagger | http://localhost:5045/swagger |
| WebUI | http://localhost:5120 |

SQL şeması açılışta `EnsureCreated` ile oluşur (migration yok).

### Deneme

UI’den sipariş vermeden önce Swagger’da en az bir pizza ve bir kurye oluşturun.

**Menü** — `POST http://localhost:5107/api/menu`

```json
{
  "name": "Margherita",
  "description": "Domates, mozzarella, fesleğen",
  "basePrice": 220,
  "isAvailable": true
}
```

Cevaptaki `id` değerini kopyalayın.

**Kurye** — `POST http://localhost:5045/api/couriers`

```json
{
  "fullName": "Ali Kurye",
  "phone": "05551112233",
  "vehiclePlate": "34 ABC 123",
  "isActive": true
}
```

**WebUI** — http://localhost:5120  
Müşteri kimliği gir, pizza seç, sepete ekle, ad + adres, **Sipariş ver**. Durum sayfası açılır.

**Kitchen** — `{orderId}` yerine UI’deki numarayı yaz:

- `POST http://localhost:5041/api/kitchen/tasks/{orderId}/start-oven`
- `POST http://localhost:5041/api/kitchen/tasks/{orderId}/ready`

WebUI’de durumu yenile.

**Delivery**

- `GET http://localhost:5045/api/deliveries/{orderId}`
- `POST http://localhost:5045/api/deliveries/{orderId}/pickup`
- `POST http://localhost:5045/api/deliveries/{orderId}/complete`

Kitchen veya RabbitMQ kapalıysa sipariş Ordering’de kalır, mutfak işi oluşmaz.

### Bilerek yok

Kimlik doğrulama, otomatik test, outbox, EF migration, WebUI’de mutfak/kurye ekranı yok.

## License / Lisans

Educational sample. / Eğitim örneği.
