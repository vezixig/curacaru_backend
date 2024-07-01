namespace Curacaru.Backend.Application.CQRS.ContactForm;

using AutoMapper;
using Core.DTO.ContactForm;
using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Infrastructure.Repositories;
using MediatR;

public class AddContactInfoRequest(Guid contactFormId, AddContactInfoDto data) : IRequest
{
    public Guid ContactFormId { get; } = contactFormId;

    public AddContactInfoDto Data { get; } = data;
}

internal class AddContactInfoRequestHandler(
    IAddressRepository addressRepository,
    ICustomerRepository customerRepository,
    IContactFormRepository contactFormRepository,
    IMapper mapper,
    IProductRepository productRepository)
    : IRequestHandler<AddContactInfoRequest>
{
    public async Task Handle(AddContactInfoRequest request, CancellationToken cancellationToken)
    {
        var contactForm = await contactFormRepository.GetContactForm(request.ContactFormId)
                          ?? throw new NotFoundException("Das Unternehmen konnte nicht gefunden werden");

        _ = await addressRepository.GetCityAsync(request.Data.ZipCode) ?? throw new NotFoundException("Die Postleitzahl konnte nicht gefunden werden");

        var products = await productRepository.GetProducts(request.Data.Products);
        if (products.Count != request.Data.Products.Count) throw new BadRequestException("Ein oder mehrere Services konnten nicht gefunden werden");

        var customer = mapper.Map<Customer>(request.Data);
        customer.Products = products;
        customer.Status = CustomerStatus.Interested;
        customer.CompanyId = contactForm.CompanyId;

        await customerRepository.AddCustomerAsync(customer);
    }
}