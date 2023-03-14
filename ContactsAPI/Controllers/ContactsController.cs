using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext _context;

        public ContactsController(ContactsAPIDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await _context.Contacts.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest request)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = request.Address,
                FullName = request.FullName,
                Phone = request.Phone,
                Email = request.Email,
            };

            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();

            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest request)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if(contact != null)
            {
                contact.Email = request.Email;
                contact.Phone = request.Phone;
                contact.FullName = request.FullName;
                contact.Address = request.Address;

                await _context.SaveChangesAsync();

                return Ok(contact);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if(contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();

                return Ok(contact);
            }

            return NotFound();
        }
    }
}
