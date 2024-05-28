namespace Curacaru.Backend.Application.CQRS.ContactForm;

using Core.DTO.ContactForm;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class UpdateContactFormRequest(User user, UpdateContactFormDto data) : IRequest
{
    public UpdateContactFormDto Data { get; } = data;

    public User User { get; } = user;
}

internal class UpdateContactFormRequestHandler(IContactFormRepository contactFormRepository) : IRequestHandler<UpdateContactFormRequest>
{
    public async Task Handle(UpdateContactFormRequest request, CancellationToken cancellationToken)
    {
        var contactForm = await contactFormRepository.GetContactFormOfCompany(request.User.CompanyId)
                          ?? throw new BadRequestException("Contact form not active");

        contactForm.Color = request.Data.Color;
        contactForm.FontSize = request.Data.FontSize;
        contactForm.IsRounded = request.Data.IsRounded;

        await contactFormRepository.UpdateContactFormAsync(contactForm);
    }
}