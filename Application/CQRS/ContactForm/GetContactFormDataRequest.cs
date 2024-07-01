namespace Curacaru.Backend.Application.CQRS.ContactForm;

using Core.DTO.ContactForm;
using Core.Exceptions;
using Infrastructure.Repositories;
using MediatR;

public class IsIdValidRequest(Guid id) : IRequest<GetContactFormDto>
{
    public Guid Id { get; } = id;
}

internal class IsIdValidRequestHandler(IContactFormRepository contactFormRepository) : IRequestHandler<IsIdValidRequest, GetContactFormDto>
{
    public async Task<GetContactFormDto> Handle(IsIdValidRequest request, CancellationToken cancellationToken)
    {
        var result = await contactFormRepository.GetContactForm(request.Id)
                     ?? throw new NotFoundException("Kein Formular mit dieser ID gefunden.");
        return new(result.Color, result.FontSize, result.Id, result.IsRounded);
    }
}