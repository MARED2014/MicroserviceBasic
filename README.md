# PizzaPulse

.NET mikroservis öğrenme projesi. Pizza siparişi **Ordering** servisinde oluşur, **MassTransit + RabbitMQ** ile **Kitchen**’a gider, pişince **Delivery** kurye atar. Servis içi komut/sorgular **MediatR (CQRS)** ile yürür.

Bu depo üretim hazır bir ürün değil; katmanlı mimari, event tabanlı iletişim ve poliglot persistence (SQL / Redis / MongoDB) denemesi içindir.

## Teknolojiler

| Katman | Teknoloji |
|--------|-----------|
| Runtime | .NET 10, ASP.NET Core |
| CQRS | MediatR |
| Mesajlaşma | MassTransit, RabbitMQ |
| Ordering | SQL Server (EF Core), Redis (sepet) |
| Kitchen | MongoDB |
| Delivery | SQL Server (EF Core), Redis (kurye müsaitlik) |
| API belgesi | Swagger (Swashbuckle) |
| UI | ASP.NET Core MVC (`PizzaPulse.WebUI`) |
| Altyapı | Docker Compose |

## Mimari

Her servis Clean Architecture katmanlarını kullanır: **Api → Application → Core**, **Infrastructure → Core**. Servisler birbirinin entity’sini paylaşmaz. Ortak olan yalnızca `PizzaPulse.BuildingBlocks` içindeki integration event’lerdir.

```text
WebUI  --HTTP-->  Ordering.Api
                      |  OrderPlaced
                      v
                 Kitchen.Api
                      |  OrderBaked
                      +-------> Ordering (status)
                      +-------> Delivery.Api (kurye)
                                    |  OrderOnTheWay / OrderDelivered
                                    v
                               Ordering (status)
```

Kuyruk adları servis önekli üretilir (`ordering-order-baked`, `delivery-order-baked`) böylece aynı event iki servise de düşer.

## Çözüm yapısı

```text
BuildingBlocks/     PizzaPulse.BuildingBlocks   (event sözleşmeleri)
Services/Ordering/  Api, Application, Core, Infrastructure
Services/Kitchen/   Api, Application, Core, Infrastructure
Services/Delivery/  Api, Application, Core, Infrastructure
UI/                 PizzaPulse.WebUI
```

## Önkoşullar

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- İsteğe bağlı: Visual Studio 2022 / Cursor

## Çalıştırma

### 1. Altyapı

Repo kökünde:

```bash
docker compose up -d
```

Ayağa kalkanlar: SQL Server `1433`, Redis `6379`, MongoDB `27017`, RabbitMQ `5672` (yönetim UI: http://localhost:15672 — `admin` / `Password123!`).

Kimlik bilgileri `appsettings.json` dosyalarıyla aynıdır. **Yalnızca yerel geliştirme içindir; production’a bu şifrelerle çıkmayın.**

### 2. Servisler

Dört uygulamayı da `http` profiliyle başlatın (ayrı terminaller):

```bash
dotnet run --project PizzaPulse.Ordering.Api --launch-profile http
dotnet run --project PizzaPulse.Kitchen.Api --launch-profile http
dotnet run --project PizzaPulse.Delivery.Api --launch-profile http
dotnet run --project PizzaPulse.WebUI --launch-profile http
```

Visual Studio’da solution’ı açıp Multiple startup projects ile dört projeyi birden çalıştırabilirsiniz. Solution dosyası: `MicroserviceBasic.slnx`.

| Uygulama | Adres |
|----------|--------|
| Ordering Swagger | http://localhost:5107/swagger |
| Kitchen Swagger | http://localhost:5041/swagger |
| Delivery Swagger | http://localhost:5045/swagger |
| WebUI (menü / sepet) | http://localhost:5120 |

Şema: Ordering ve Delivery açılışta `EnsureCreated` ile SQL tablolarını oluşturur. EF migration yoktur.

## Senaryo (Swagger)

Menü seed edilmez; sıfırdan kayıt açın. Aşağıdaki gövdeleri Try it out ile kullanın. Dönen `id` / `orderId` değerlerini sonraki isteklerde değiştirin.

**1. Menü oluştur —** `POST http://localhost:5107/api/menu`

```json
{
  "name": "Margherita",
  "description": "Domates sosu, mozzarella, fesleğen",
  "basePrice": 220,
  "isAvailable": true
}
```

İkinci pizza için aynı endpoint’i başka bir isimle tekrarlayın. `GET /api/menu` ile Id’leri alın.

**2. Sepete ekle —** `POST http://localhost:5107/api/cart/items`

```json
{
  "customerId": "musteri-1",
  "pizzaMenuId": "BURAYA-MENU-ID",
  "quantity": 1,
  "size": "Medium"
}
```

**3. Sepeti gör —** `GET http://localhost:5107/api/cart?customerId=musteri-1`

**4. Sipariş ver —** `POST http://localhost:5107/api/orders`

```json
{
  "customerId": "musteri-1",
  "customerName": "Ali Veli",
  "deliveryAddress": "Kadıköy, İstanbul"
}
```

Cevaptaki `orderId` Kitchen’a `OrderPlaced` event’i ile gider. `GET http://localhost:5041/api/kitchen/tasks/{orderId}` ile iş emrini kontrol edin.

**5. Fırın / hazır —** body yok

- `POST http://localhost:5041/api/kitchen/tasks/{orderId}/start-oven`
- `POST http://localhost:5041/api/kitchen/tasks/{orderId}/ready`

Hazır olunca Delivery `OrderBaked` dinler ve kurye atamaya çalışır.

**6. Kurye oluştur —** `POST http://localhost:5045/api/couriers` (siparişi pişirmeden önce)

```json
{
  "fullName": "Ali Kurye",
  "phone": "05551112233",
  "vehiclePlate": "34 ABC 123",
  "isActive": true
}
```

Liste: `GET /api/couriers` veya `GET /api/couriers/active`. Güncelleme `PUT /api/couriers/{id}`, silme (`IsActive = false`) `DELETE /api/couriers/{id}`.

**7. Teslimat**

- `GET http://localhost:5045/api/deliveries/{orderId}`
- `POST http://localhost:5045/api/deliveries/{orderId}/pickup`
- `POST http://localhost:5045/api/deliveries/{orderId}/complete`

Sipariş durumu Ordering’de event’lerle güncellenir: `Received → Preparing → Baked → OnTheWay → Delivered`.

WebUI menü, sepet **ve sipariş vermeyi** kapsar. Sipariş sonrası durum sayfasından takip edilir. Fırın / hazır / kurye alma-teslim operatör adımları Swagger’dadır.

## Bilinen sınırlar

Öğrenme iskeleti olduğu için şunlar kasıtlı olarak eksik veya kaba bırakılmıştır:

- Event publish ile veritabanı yazımı aynı transaction’da değil (outbox yok).
- Sipariş status güncellemesi geriye sarılabilir (ör. `OnTheWay` iken tekrar `OrderBaked` gelirse).
- EF Core migration yok; şema `EnsureCreated` ile gelir.
- Otomatik test ve kimlik doğrulama yok.

## Lisans

Örnek / eğitim amaçlıdır.
