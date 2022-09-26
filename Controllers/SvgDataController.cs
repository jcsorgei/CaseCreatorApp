using System.Threading.Tasks;
using CaseCreatorApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

[ApiController]
[Route("[controller]")]
public class SvgDataController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    public SvgDataController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var userId = _userManager.GetUserId(User);

        var svgs = await _context.SvgData
            .Where(s => s.AppUserId == userId)
            .Include(s => s.Images)
                .ThenInclude(i => i.CurrentTransformation)
            .Include(s => s.Texts)
                .ThenInclude(t => t.CurrentTransformation)
            .ToListAsync();

        return Ok(svgs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var userId = _userManager.GetUserId(User);
        var svg = await _context.SvgData
            .Where(s => s.AppUserId == userId)
            .Include(s => s.Images)
                .ThenInclude(i => i.CurrentTransformation)
            .Include(s => s.Texts)
                .ThenInclude(i => i.CurrentTransformation)
            .FirstOrDefaultAsync(s => s.Id == id);

        return Ok(svg);
    }

    [HttpPost]
    public async Task<IActionResult> Post(SvgData svgData)
    {
        var userId = _userManager.GetUserId(User);

        svgData.AppUserId = userId;
        _context.Add(svgData);
        await _context.SaveChangesAsync();

        return Ok("Svg added to db!");

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(int id, SvgData svgData)
    {
        // var userId = _userManager.GetUserId(User);

        // svgData.AppUserId = userId;

        var newSvgData = await _context.SvgData
            .Include(s => s.Images)
            .Include(s => s.Texts)
            .FirstOrDefaultAsync(s => s.Id == id);


        if (newSvgData == null) return NotFound();

        newSvgData.Name = svgData.Name;
        newSvgData.Base64 = svgData.Base64;
        newSvgData.Svg = svgData.Svg;
        newSvgData.TemplateId = svgData.TemplateId;
        newSvgData.TemplateColor = svgData.TemplateColor;

        foreach (var image in newSvgData.Images)
        {
            _context.Images.Remove(image);
        }

        foreach (var text in newSvgData.Texts)
        {
            _context.Texts.Remove(text);
        }

        foreach (var newImage in svgData.Images)
        {
            newSvgData.Images.Add(newImage);
        }

        foreach (var newText in svgData.Texts)
        {
            newSvgData.Texts.Add(newText);
        }

        _context.SaveChanges();

        return Ok("Svg edited!");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Console.WriteLine("TESTING PARAM:" + id);
        var deletedSvg = await _context.SvgData
            .Include(s => s.Images)
                .ThenInclude(i => i.CurrentTransformation)
            .Include(s => s.Texts)
                .ThenInclude(t => t.CurrentTransformation)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (deletedSvg == null) return NotFound();

        _context.Remove(deletedSvg);
        await _context.SaveChangesAsync();

        return Ok("Svg deleted!");
    }


}