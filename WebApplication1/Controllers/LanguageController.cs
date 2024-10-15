using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication1.DTOs;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        public LanguageController(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LanguageItem>))]
        public IActionResult GetLanguages()
        {
            var languages = _languageRepository.GetLanguages();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(languages);
        }

        [HttpGet("{languageId}")]
        [ProducesResponseType(200, Type = typeof(LanguageItem))]
        [ProducesResponseType(400)]
        public IActionResult GetLanguage(int languageId) 
        {
            if(!_languageRepository.LanguageExists(languageId))
            {
                return NotFound();
            }

            var language = _languageRepository.GetLanguageById(languageId);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(language);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateLanguage([FromBody] LanguagePost languageCreate)
        {
            if (languageCreate == null)
            {
                return BadRequest(ModelState);
            }

            var languages = _languageRepository.GetLanguages().Where(c => c.Name.Trim().ToUpper() == 
            languageCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (languages != null)
            {
                ModelState.AddModelError("", "Язык уже существует");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LanguageItem languageCurrent = new LanguageItem();
            languageCurrent.Name = languageCreate.Name;
            languageCurrent.Description = languageCreate.Description;
            languageCurrent.LenghtOfCourse = languageCreate.LenghtOfCourse;

            if (!_languageRepository.CreateLanguage(languageCurrent))
            {
                ModelState.AddModelError("", "Что-то пошло не так");
                return StatusCode(500,ModelState);
            }

            return Ok("Удачное добавление");
        }

        [HttpPut("{languageId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateLanguage(int languageId, [FromBody]LanguagePost updateLanguage)
        {
            if (updateLanguage == null)
            {
                return BadRequest(ModelState);
            }

            if (!_languageRepository.LanguageExists(languageId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LanguageItem languageCurrent = new LanguageItem();
            languageCurrent.Name = updateLanguage.Name;
            languageCurrent.Description = updateLanguage.Description;
            languageCurrent.LenghtOfCourse = updateLanguage.LenghtOfCourse;
            languageCurrent.Id = languageId;    

            if (!_languageRepository.UpdateLanguage(languageCurrent))
            {
                ModelState.AddModelError("", "Что-то пошло не так при обновлении");
                return BadRequest(ModelState);  
            }

            return NoContent();
        }

        [HttpDelete("{languageId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteLanguage(int languageId)
        {
            if (!_languageRepository.LanguageExists(languageId))
            {
                return NotFound();
            }

            var languageToDelete = _languageRepository.GetLanguageById(languageId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_languageRepository.DeleteLanguage(languageToDelete))
            {
                ModelState.AddModelError("", "Что-то пошло не так при удалении");
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("deleteAll")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(500)] 
        public IActionResult DeleteAllLanguages()
        {
            if (!_languageRepository.DeleteAll())
            {
                ModelState.AddModelError("", "Что-то пошло не так при удалении всех записей");
                return StatusCode(500, ModelState);
            }

            return NoContent(); 
        }
    }
}
