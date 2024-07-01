namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

public interface IContactFormRepository
{
    /// <summary>Adds a new contact form.</summary>
    /// <param name="contactForm">The contact form to add.</param>
    Task AddContactFormAsync(ContactForm contactForm);

    /// <summary>Gets the contact form by its id.</summary>
    /// <param name="id">The contact form id.</param>
    /// <returns>The contact form.</returns>
    Task<ContactForm?> GetContactForm(Guid id);

    /// <summary>Gets the contact form of a company.</summary>
    /// <param name="companyId">The company id.</param>
    /// <returns>The contact form or null if none is found.</returns>
    Task<ContactForm?> GetContactFormOfCompany(Guid companyId);

    /// <summary>Updates a contact form.</summary>
    /// <param name="contactForm">The modified contact form.</param>
    /// <returns>An awaitable task object.</returns>
    Task UpdateContactFormAsync(ContactForm contactForm);
}