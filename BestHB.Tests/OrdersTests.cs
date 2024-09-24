using BestHB.Domain.Commands;
using BestHB.Domain.Entities;
using BestHB.Domain.Repositories;
using BestHB.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BestHB.Tests;

[TestClass]
public class OrdersTest
{
    [TestMethod]
    public void invalid_lot_size_on_create_order_test()
    {
        //given
        CreateOrderCommand command = new()
        {
            Price = 10,
            Quantity = 40,
            Side = OrderSide.Sell,
            Symbol = "PETR4",
            Type = OrderType.Market,
            UserId = 123
        };

        InstrumentInfo instrumentInfo = new()
        {
            Type = InstrumentType.Stock,
            Symbol = "PETR4",
            Description = "PETROBRAS",
            Exchange = "BOVESPA",
            ISIN = "123456",
            LotStep = 100,
            MaxLot = 100000,
            MinLot = 100
        };

        var instrumentInfoRepositoryMock = new Mock<IRepository>();

        instrumentInfoRepositoryMock.Setup(i => i.Get(It.IsAny<string>())).ReturnsAsync(instrumentInfo);

        var orderRepositoryMock = Mock.Of<IRepository>();

        var message = string.Empty;

        try
        {
            var orderService = new OrderService(orderRepositoryMock, instrumentInfoRepositoryMock.Object);

            orderService.Create(command);
        }
        catch (Exception ex)
        {
            message = ex.Message;
        }

        Assert.AreEqual("Quantidade inválida.", message);
    }
}
