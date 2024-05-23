namespace Curacaru.Backend.Application.CQRS.ContactForm;

using Core.DTO.ContactForm;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class GetContactFormOfCompanyRequest(User user) : IRequest<GetContactFormDto?>
{
    public User User { get; } = user;
}

internal class GetContactFormOfCompanyRequestHandler(IContactFormRepository contactFormRepository)
    : IRequestHandler<GetContactFormOfCompanyRequest, GetContactFormDto?>
{
    public async Task<GetContactFormDto?> Handle(GetContactFormOfCompanyRequest request, CancellationToken cancellationToken)
    {
        var contactForm = await contactFormRepository.GetContactFormOfCompany(request.User.CompanyId);
        return contactForm is null ? null : new(contactForm.Color, contactForm.FontSize, contactForm.Id);
    }
}