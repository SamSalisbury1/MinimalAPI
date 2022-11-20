using Microsoft.EntityFrameworkCore;
using BasicAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PetDb>(opt => opt.UseInMemoryDatabase("PetList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

RouteGroupBuilder petItems = app.MapGroup("/petitems");

petItems.MapGet("/", GetAllPets);
petItems.MapGet("/{id}", GetPet);
petItems.MapPost("/", CreatePet);
petItems.MapPut("/{id}", UpdatePet);
petItems.MapDelete("/{id}", DeletePet);

app.Run();

static async Task<IResult> GetAllPets(PetDb db)
{
    return TypedResults.Ok(await db.Pets.Select(x => new PetItemDTO(x)).ToArrayAsync());
}

static async Task<IResult> GetPet(int id, PetDb db)
{
    return await db.Pets.FindAsync(id)
        is Pet pet
            ? TypedResults.Ok(new PetItemDTO(pet))
            : TypedResults.NotFound();
}

static async Task<IResult> CreatePet(PetItemDTO petItemDTO, PetDb db)
{
    var petItem = new Pet
    {
        OwnerName = petItemDTO.OwnerName,
        HasOwner = petItemDTO.HasOwner,
        Animal = petItemDTO.Animal,
        Name = petItemDTO.Name
    };

    db.Pets.Add(petItem);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/petitems/{petItem.Id}", petItemDTO);
}

static async Task<IResult> UpdatePet(int id, PetItemDTO petItemDTO, PetDb db)
{
    var pet = await db.Pets.FindAsync(id);

    if (pet is null) return TypedResults.NotFound();
    
    pet.Name = petItemDTO.Name;
    pet.Animal = petItemDTO.Animal;
    pet.HasOwner = petItemDTO.HasOwner;
    pet.OwnerName = petItemDTO.OwnerName;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeletePet(int id, PetDb db)
{
    if (await db.Pets.FindAsync(id) is Pet pet)
    {
        db.Pets.Remove(pet);
        await db.SaveChangesAsync();
        return TypedResults.Ok(pet);
    }

    return TypedResults.NotFound();
}