namespace FlmAutoRent.Data.Entities
{
    public class VehiclesImagesMapping
    {
        public int Id { get; set; }
        public VehiclesImage Image { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}