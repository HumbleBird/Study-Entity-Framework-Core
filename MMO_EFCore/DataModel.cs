﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace MMO_EFCore
{
    [Table("Item")]
    public class Item
    {
        public bool SoftDeleted { get; set; }

        // 이름Id -> PK
        public int ItemId { get; set; }
        public int TemplateId { get; set; } // 101 = 집행검
        public DateTime CreateDate { get; private set; }

        public int Itemgrade { get; set; }

        // 다른 클래스 참조 -> FK (Navigational Property)

        public int OwnerId { get; set; }
        public Player Owner { get; set; }
    }

    public interface ILogEntity
    {
        DateTime CreateTime { get; }
        void SetCreateTime();
    }

    // Entity 클래스 이름 = 테이블 이름 = Player	
    [Table("Player")]
    public class Player : ILogEntity
    {
        // 이름Id -> PK
        public int PlayerId { get; set; }

        [Required]
        [MaxLength(20)]
        // Alternate Key
        public string Name { get; set; }
        public Item OwnedItem { get; set; }
        public Guild Guild { get; set; }

        public DateTime CreateTime { get; private set; }

        public void SetCreateTime()
        {
            CreateTime = DateTime.Now;
        }
    }

    [Table("Guild")]
    public class Guild
    {
        public int GuildId { get; set; }
        public string GuildName { get; set; }
        public ICollection<Player> Members { get; set; }
    }

    // DTO (Data Transfer Object)
    public class GuildDto
    {
        public int GuildId { get; set; }
        public string Name { get; set; }
        public int MemberCount { get; set; }
    }
}
