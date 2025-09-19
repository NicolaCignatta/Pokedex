using Pokedex.Domain.Aggregates;
using Pokedex.Domain.ValueObjects;

namespace Pokedex.UnitTests
{
    public class PokemonTests
    {
        [Fact]
        public void Materialize_WithValidData_ReturnsPokemon()
        {
            var pokemon = Pokemon.Materialize(1, "Pikachu", "Electric mouse", "forest", false);

            Assert.NotNull(pokemon);
            Assert.Equal(1, pokemon.Id);
            Assert.Equal("Pikachu", pokemon.Name);
            Assert.Equal("Electric mouse", pokemon.Description);
            Assert.Equal("forest", pokemon.HabitatName);
            Assert.False(pokemon.IsLegendary);
        }

        [Theory]
        [InlineData(0, "Pikachu", "desc", "forest")]
        [InlineData(-1, "Pikachu", "desc", "forest")]
        [InlineData(1, "", "desc", "forest")]
        [InlineData(1, "Pikachu", "", "forest")]
        [InlineData(1, "Pikachu", "desc", "")]
        public void Materialize_WithInvalidData_ReturnsNull(int id, string name, string desc, string habitat)
        {
            var pokemon = Pokemon.Materialize(id, name, desc, habitat, false);

            Assert.Null(pokemon);
        }

        [Fact]
        public void TranslateDescription_WithValidNewValue_UpdatesDescription()
        {
            var pokemon = Pokemon.Materialize(1, "Pikachu", "Electric mouse", "forest", false)!;

            pokemon.TranslateDescription("Nuova descrizione");

            Assert.Equal("Nuova descrizione", pokemon.Description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Electric mouse")]
        public void TranslateDescription_WithInvalidOrSameValue_DoesNothing(string newDescription)
        {
            var pokemon = Pokemon.Materialize(1, "Pikachu", "Electric mouse", "forest", false)!;

            pokemon.TranslateDescription(newDescription);

            Assert.Equal("Electric mouse", pokemon.Description);
        }

        [Fact]
        public void MyLanguage_ReturnsYoda_WhenLegendary()
        {
            var pokemon = Pokemon.Materialize(1, "Mewtwo", "desc", "forest", true)!;

            Assert.Equal(Language.Yoda, pokemon.MyLanguage);
        }

        [Fact]
        public void MyLanguage_ReturnsYoda_WhenHabitatIsCave()
        {
            var pokemon = Pokemon.Materialize(1, "Zubat", "desc", "cave", false)!;

            Assert.Equal(Language.Yoda, pokemon.MyLanguage);
        }

        [Fact]
        public void MyLanguage_ReturnsShakespeare_Otherwise()
        {
            var pokemon = Pokemon.Materialize(1, "Pikachu", "desc", "forest", false)!;

            Assert.Equal(Language.Shakespeare, pokemon.MyLanguage);
        }
    }
}