using GoFish.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace GoFish.Data.Entities
{
	public class AppUser : IdentityUser<Guid>
	{
		public string Name { get; set; }

        // Navigation
        public virtual ICollection<GamePlayer> GamePlayers { get; set; }
    }
}
