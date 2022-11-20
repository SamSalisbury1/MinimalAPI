using Microsoft.EntityFrameworkCore;
using BasicAPI;

public class PetDb: DbContext
{
    public PetDb(DbContextOptions<PetDb> options)
    : base(options) { }

    public DbSet<Pet> Pets => Set<Pet>();
}
