// See https://aka.ms/new-console-template for more information

using CartWebApp.Interfaces;
using CartWebApp.Service;
using DomainClasses.Dtos.Cart;

ICartService cartService = new CartService();
var res=await cartService.AddToCart(new AddProductToCartDto() { ProductId = 1, Quantity = 5 });
Console.WriteLine("Hello, World!");
