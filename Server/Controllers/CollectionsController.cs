using GgdbNet.Server.Models;
using GgdbNet.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GgdbNet.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollectionsController : ControllerBase
{
    private readonly GgdbContext db;

    public CollectionsController(GgdbContext db)
    {
        this.db = db;
    }

    [HttpGet("{publicId}")]
    public async Task<IActionResult> GetCollection(string publicId)
        => await db.Collections
            .Where(c => c.PublicId == publicId)
            .Include(c => c.Games)
            .FirstOrDefaultAsync() is { } col
            ? Ok(col)
            : NotFound();
    
    [HttpPost]
    public async Task SaveCollection(Collection col)
    {
        if (await db.Collections
                .Where(c => c.PublicId == col.PublicId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync() is int colId)
        {
            col.Id = colId;
            await db.Games.Where(g => g.CollectionId == colId).DeleteFromQueryAsync();
        }

        db.Collections.Update(col);
        await db.SaveChangesAsync();
    }
}