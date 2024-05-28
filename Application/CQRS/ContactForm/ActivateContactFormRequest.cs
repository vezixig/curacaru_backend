namespace Curacaru.Backend.Application.CQRS.ContactForm;

using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class ActivateContactFormRequest(User user) : IRequest
{
    public User User { get; } = user;
}

internal class ActivateContactFormRequestHandler(IContactFormRepository contactFormRepository) : IRequestHandler<ActivateContactFormRequest>
{
    public Task Handle(ActivateContactFormRequest request, CancellationToken cancellationToken)
        => contactFormRepository.AddContactFormAsync(
            new()
            {
                CompanyId = request.User.CompanyId,
                Color = "255,127,2",
                FontSize = 12,
                IsRounded = true
            });
}