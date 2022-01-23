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
    public class HistoricosController : Controller
    {
        private readonly HistoricoService _historicoService;
        private readonly PapelService _papelService;

        public HistoricosController(HistoricoService historicoService, PapelService papelService)
        {
            _historicoService = historicoService;
            _papelService = papelService;
        }

        // Carteira GET
        public async Task<IActionResult> Index(DateTime? dataMin, DateTime? dataMax)
        {
            if (!dataMin.HasValue)
            {
                dataMin = new DateTime(2021, 1, 1);
            }
            if (!dataMax.HasValue)
            {
                dataMax = DateTime.Now;
            }
            ViewData["dataMin"] = dataMin.Value.ToString("yyyy-MM-dd");
            ViewData["dataMax"] = dataMax.Value.ToString("yyyy-MM-dd");

            var resultado = await _historicoService.BuscaPorData(dataMin, dataMax);
            return View(resultado);
        }

        // Detalhes GET
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fonecido!" });
            }

            var obj = await _historicoService.BuscarId(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Operação não encontrada!" });
            }
            return View(obj);
        }

        // Criar GET
        public async Task<IActionResult> Create()
        {
            var papeis = await _papelService.BuscarTodos();
            ViewBag.Papeis = papeis;
            return View();
        }

        // Criar POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Historico historico)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var papeis = await _papelService.BuscarTodos();
                    ViewBag.Papeis = papeis;
                }
                await _historicoService.Inserir(historico);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // editar GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido!" });
            }

            var obj = await _historicoService.BuscarId(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Operação não encontrada!" });
            }

            List<Papel> papeis = await _papelService.BuscarTodos();
            ViewBag.Papeis = papeis;

            return View(obj);
        }

        // editar POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Historico historico)
        {
            if (id != historico.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não corresponde!" });
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var papeis = await _papelService.BuscarTodos();
                    ViewBag.Papeis = papeis;
                }
                await _historicoService.Editar(historico);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // excluir GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido!" });
            }

            var obj = await _historicoService.BuscarId(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Operação não encontrada!" });
            }
            return View(obj);
        }

        // excluir POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _historicoService.Remover(id);
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
