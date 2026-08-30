using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.BuildingBlocks.Comman;

public class EventBusConstant
{
    public const string OrderPlacedQueue = "order-placed-queue";
    public const string OrderPreparationStartedQueue = "order-preparation-started-queue";
    public const string OrderBakedQueue = "order-baked-queue";
    public const string OrderOnTheWayQueue = "order-on-the-way-queue";
    public const string OrderDeliveredQueue = "order-delivered-queue";
}
