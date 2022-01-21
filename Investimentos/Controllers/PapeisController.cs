using Investimentos.Models;
using Investimentos.Services;
using Investimentos.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Investimentos.Controllers
{
    public class PapeisController : Controller
    {
        private readonly PapelService _papelService;
        private readonly TipoPapelService _tipoPapelService;

        public PapeisController(PapelService papelService, TipoPapelService tipoPapelService)
        {
            _papelService = papelService;
            _tipoPapelService = tipoPapelService;
        }

        // Papeis GET
        public async Task<IActionResult> Index()
        {
            var list = await _papelService.BuscarTodos();
            return View(list);
        }

        // Detalhes GET
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fonecido!" });
            }

            var papel = await _papelService.BuscarId(id.Value);
            if (papel == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Papel não encontrado!" });
            }
            return View(papel);
        }

        // Criar GET
        public async Task<IActionResult> Create()
        {
            var tipoPapeis = await _tipoPapelService.BuscarTodos();
            ViewBag.TipoPapeis = tipoPapeis;
            return View();
        }

        // Criar POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Papel papel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tipoPapeis = await _tipoPapelService.BuscarTodos();
                    ViewBag.TipoPapeis = tipoPapeis;
                }
                await _papelService.Inserir(papel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // Editar GET
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido!" });
            }

            var papel = await _papelService.BuscarId(id.Value);
            if (papel == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Papel não encontrado!" });
            }

            List<TipoPapel> tipoPapeis = await _tipoPapelService.BuscarTodos();
            ViewBag.TipoPapeis = tipoPapeis;
            
            return View(papel);
        }

        // Editar POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Papel papel)
        {
            if(id != papel.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não corresponde!" });
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var tipoPapeis = await _tipoPapelService.BuscarTodos();
                    ViewBag.TipoPapeis = tipoPapeis;
                }
                await _papelService.Editar(papel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // Excluir GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido!" });
            }

            var papel = await _papelService.BuscarId(id.Value);
            if (papel == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Papel não encontrado!" });
            }
            return View(papel);
        }

        // Excluir POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _papelService.Remover(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // Error
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}