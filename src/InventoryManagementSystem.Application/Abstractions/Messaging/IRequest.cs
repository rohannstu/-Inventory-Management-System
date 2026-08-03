namespace InventoryManagementSystem.Application.Abstractions.Messaging;

// Marker interface — no members. Its only job is to let the mediator
// and DI container recognize "this type is a dispatchable request."
public interface IRequest<TResponse> { }