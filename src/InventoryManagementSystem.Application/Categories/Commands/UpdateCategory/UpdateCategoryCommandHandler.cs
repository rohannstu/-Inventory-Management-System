using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;

namespace InventoryManagementSystem.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCategoryCommand, bool>
{
    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
            return false;

        if (!string.Equals(category.Name, request.Name, StringComparison.OrdinalIgnoreCase)
            && await categoryRepository.ExistsByNameAsync(request.Name, cancellationToken))
        {
            throw new InvalidOperationException($"A category with name '{request.Name}' already exists.");
        }

        category.Rename(request.Name);
        category.UpdateDescription(request.Description);

        await categoryRepository.UpdateAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
