using Hibernum.Server.Services;
using Hibernum.Server.Services.Rules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Dims;
using Server.Dtos;
using Server.Exceptions;
using Server.Validation;

namespace Hibernum.Server.Controllers;

[Authorize]
public class FileController: ControllerBase
{
    private FileService _fileService;

    public FileController(FileService fileService)
    { this._fileService = fileService; }

    [HttpGet]
    public IActionResult Index(
        [FromQuery] long? id,
        [FromQuery] string? name,
        [FromQuery] string? mime
    ) {
        var results = this._fileService.Find(new FindFileRule()
        { 
            Id = id,
            Name = name,
            Mime = mime
        }).Select(a => FileDTO.ByEntity(a));

        return Ok(results);
    }

    [HttpPost]
    public IActionResult Index(
        [FromBody] FileDim dim
    ) {
        ValidateModel<FileDim>.Validate(dim);
        var file = this._fileService.Save(dim);
        
        return Ok(FileDTO.ByEntity(file));
    }

    [HttpDelete]
    public IActionResult Index([FromQuery] long? id)
    {
        if (id is null)
        {
            Dictionary<string, object> erroMessage = new();
            erroMessage.Add("Id", "required");
            throw new ServerValidationException(erroMessage);
        }

        return Ok(
            FileDTO.ByEntity(this._fileService.Remove(id ?? 0))
        );
    }
}