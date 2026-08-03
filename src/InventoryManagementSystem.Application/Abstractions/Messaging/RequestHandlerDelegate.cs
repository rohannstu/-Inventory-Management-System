namespace InventoryManagementSystem.Application.Abstractions.Messaging;

// Represents "the rest of the pipeline" from a given behavior's point of view —
// either the next behavior in the chain, or the real handler if there's nothing left.
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();