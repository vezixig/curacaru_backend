namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Attributes;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

[Repository]
internal class ContactFormRepository(DataContext dataContext) : IContactFormRepository
{
    public Task AddContactFormAsync(ContactForm contactForm)
    {
        dataContext.ContactForms.Add(contactForm);
        return dataContext.SaveChangesAsync();
    }

    public Task<ContactForm?> GetContactForm(Guid id)
        => dataContext.ContactForms.FirstOrDefaultAsync(c => c.Id == id);

    public Task<ContactForm?> GetContactFormOfCompany(Guid companyId)
        => dataContext.ContactForms.FirstOrDefaultAsync(c => c.CompanyId == companyId);

    public Task UpdateContactFormAsync(ContactForm contactForm)
    {
        dataContext.ContactForms.Update(contactForm);
        return dataContext.SaveChangesAsync();
    }
}