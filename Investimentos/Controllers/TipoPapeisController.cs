using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Investimentos.Data;
using Investimentos.Models;
using Investimentos.Services;
using Investimentos.Services.Exceptions;
using System.Diagnostics;

namespace Investimentos.Controllers
{
    public class TipoPapeisController : Controller
    {
        private readonly TipoPapelService _tipoPapelService;

        public TipoPapeisController(TipoPapelService tipoPapelService)
        {
            _tipoPapelService = tipoPapelService;
        }

        // TipoPapeis GET
        public async Task<IActionResult> Index()
        {
            var list = await _tipoPapelService.BuscarTodos();
            return View(list);
        }

        // Detalhes GET
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fonecido!" });
            }

            var tipoPapel = await _tipoPapelService.BuscarId(id.Value);
            if (tipoPapel == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Tipo de Papel não encontrado!" });
            }
            return View(tipoPapel);
        }

        // Criar GET
        public IActionResult Create()
        {
            return View();
        }

        // Criar POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoPapel tipoPapel)
        {
            if (ModelState.IsValid)
            {
                await _tipoPapelService.Inserir(tipoPapel);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoPapel);
        }

        // Editar GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido!" });
            }

            var tipoPapel = await _tipoPapelService.BuscarId(id.Value);
            if (tipoPapel == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Tipo de Papel não encontrado!" });
            }
            return View(tipoPapel);
        }

        // Editar POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoPapel tipoPapel)
        {
            if (id != tipoPapel.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido!" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _tipoPapelService.Editar(tipoPapel);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    return RedirectToAction(nameof(Error), new { message = "Tipo de Papel não encontrado!" });
                }
            }
            return View(tipoPapel);
        }

        // Excluir GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido!" });
            }

            var tipoPapel = await _tipoPapelService.BuscarId(id.Value);
            if (tipoPapel == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Tipo de Papel não encontrado!" });
            }
            return View(tipoPapel);
        }

        // Excluir POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tipoPapelService.Remover(id);
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