﻿using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Models;

namespace Buk.UniversalGames.Interfaces
{
    public interface IStickerService
    {
        ScanResult ScanSticker(Team team, string code);

        List<Sticker> GetStickers(int leagueId);

        byte[]? GetStickerQRById(int stickerId, int size = 20);
    }
}