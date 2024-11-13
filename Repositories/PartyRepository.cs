using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Dto;
using Gvz.Laboratory.UserService.Entities;
using Gvz.Laboratory.UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.UserService.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly GvzLaboratoryUserServiceDbContext _context;
        private readonly IUserRepository _userRepository;
        public PartyRepository(GvzLaboratoryUserServiceDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        public async Task<Guid> CreatePartyAsync(PartyDto party)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id);
            if (existingParty == null)
            {
                var userEntity = await _userRepository.GetUserEntityByIdAsync(party.UserId)
                    ?? throw new InvalidOperationException($"User with Id '{party.UserId}' was not found.");

                var partyEntity = new PartyEntity
                {
                    Id = party.Id,
                    BatchNumber = party.BatchNumber,
                    DateOfReceipt = party.DateOfReceipt,
                    ProductName = party.ProductName,
                    SupplierName = party.SupplierName,
                    ManufacturerName = party.ManufacturerName,
                    BatchSize = party.BatchSize,
                    SampleSize = party.SampleSize,
                    TTN = party.TTN,
                    DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety,
                    TestReport = party.TestReport,
                    DateOfManufacture = party.DateOfManufacture,
                    ExpirationDate = party.ExpirationDate,
                    Packaging = party.Packaging,
                    Marking = party.Marking,
                    Result = party.Result,
                    User = userEntity,
                    Note = party.Note,
                };

                await _context.Parties.AddAsync(partyEntity);
                await _context.SaveChangesAsync();
            }

            return party.Id;
        }
        public async Task<(List<PartyModel> parties, int numberParties)> GetUserPartiesForPageAsync(Guid userId, int pageNumber)
        {
            var partyEntities = await _context.Parties
                    .AsNoTracking()
                    .Where(p => p.User.Id == userId)
                    .Include(p => p.User)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();

            if (!partyEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                partyEntities = await _context.Parties
                    .AsNoTracking()
                    .Where(p => p.User.Id == userId)
                    .Include(p => p.User)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberParties = await _context.Parties
                .Where(p => p.User.Id == userId)
                .CountAsync();

            var parties = partyEntities.Select(p => PartyModel.Create(
                p.Id,
                p.BatchNumber,
                p.DateOfReceipt,
                p.ProductName,
                p.SupplierName,
                p.ManufacturerName,
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                UserModel.Create(p.User.Id, p.User.Role, p.User.Surname, p.User.UserName, p.User.Patronymic, false).user,
                p.Note
                )).ToList();

            return (parties, numberParties);
        }
        public async Task<Guid> UpdatePartyAsync(PartyDto party)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id)
                ?? throw new InvalidOperationException($"Party with Id '{party.Id}' was not found.");

            var userEntity = await _userRepository.GetUserEntityByIdAsync(party.UserId)
                ?? throw new InvalidOperationException($"User with Id '{party.UserId}' was not found.");

            existingParty.BatchNumber = party.BatchNumber;
            existingParty.DateOfReceipt = party.DateOfReceipt;
            existingParty.ProductName = party.ProductName;
            existingParty.SupplierName = party.SupplierName;
            existingParty.ManufacturerName = party.ManufacturerName;
            existingParty.BatchSize = party.BatchSize;
            existingParty.SampleSize = party.SampleSize;
            existingParty.TTN = party.TTN;
            existingParty.DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety;
            existingParty.TestReport = party.TestReport;
            existingParty.DateOfManufacture = party.DateOfManufacture;
            existingParty.ExpirationDate = party.ExpirationDate;
            existingParty.Packaging = party.Packaging;
            existingParty.Marking = party.Marking;
            existingParty.Result = party.Result;
            existingParty.Note = party.Note;

            await _context.SaveChangesAsync();

            return party.Id;
        }

        public async Task DeletePartiesAsync(List<Guid> ids)
        {
            await _context.Parties
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
