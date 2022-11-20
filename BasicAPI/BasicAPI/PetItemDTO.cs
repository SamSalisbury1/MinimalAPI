namespace BasicAPI
{
    public class PetItemDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Animal { get; set; }
        public bool HasOwner { get; set; }
        public string? OwnerName { get; set; }

        public PetItemDTO() { }
        public PetItemDTO(Pet petItem) => (Id, Name, Animal, HasOwner, OwnerName) = (petItem.Id, petItem.Name, petItem.Animal, petItem.HasOwner, petItem.OwnerName);
    }
}
