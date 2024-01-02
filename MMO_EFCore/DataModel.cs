using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMO_EFCore
{
    public struct ItemOption
    {
        public int str;
        public int dex;
        public int hp;
    }

    [Table("Item")]
    public class Item
    {
        private string _jsonData;
        public string JsonData { get {return _jsonData ; } set {_jsonData = value; } }

        public void SetOption(ItemOption option)
        {
            _jsonData = JsonConvert.SerializeObject(option);
        }

        public ItemOption GetOption()
        {
            return JsonConvert.DeserializeObject<ItemOption>(_jsonData);
        }


        public bool SoftDeleted { get; set; }

        // 이름Id -> PK
        public int ItemId { get; set; }
        public int TemplateId { get; set; }
        public DateTime CreatedDate { get; set; }

        // 다른 클래스 참조 -> FK (Navigational Property)
        //public int OwerId { get; set; }
        //[ForeignKey("OwnerId")]
        public int? OwnerId { get; set; }
        public Player Owner { get; set; }

        public int? CreateId { get; set; }
        public Player Creator { get; set; }
    }

    // 클래스 이름 = 테이블 이름 = Player
    [Table("Player")]
    public class Player
    {
        // 이름Id -> PK
        public int PlayerId {  get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [InverseProperty("Owner")]
        public Item OwnedItem { get; set; }
        [InverseProperty("Createor")]
        public ICollection<Item> CreateItems { get; set; }

        public Guild Guild { get; set; }
    }

    [Table("Guild")]
    public class Guild
    {
        public int GuildId { get; set; }
        public string GuildName { get; set; }
        public ICollection<Player> Members { get; set; }

    }

    // DTO (Data Tranfer Object)
    public class GuildDto
    {
        public int GuildId { get; set; }
        public string Name { get; set; }
        public int MemberCount { get; set; }
    }
}
