using Investimentos.Data;
using Investimentos.Models;
using Investimentos.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investimentos.Services
{
    public class HistoricoService
    {
        private readonly InvestimentosContext _context;

        public HistoricoService(InvestimentosContext context)
        {
            _context = context;
        }

        // listar todos
        public async Task<List<Historico>> BuscarTodos()
        {
            return await _context.Historico.ToListAsync();
        }

        // buscar por id
        public async Task<Historico> BuscarId(int id)
        {
            return await _context.Historico.Include(historico => historico.Papel).FirstOrDefaultAsync(historico => historico.Id == id);
        }

        // inserir
        public async Task Inserir(Historico historico)
        {
            try
            {
                _context.Add(historico);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // editar
        public async Task Editar(Historico historico)
        {
            var obj = await _context.Historico.AnyAsync(x => x.Id == historico.Id);
            if (!obj)
            {
                throw new NotFoundException("Não encontrado!");
            }
            try
            {
                _context.Update(historico);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        // excluir
        public async Task Remover(int id)
        {
            try
            {
                var historico = await _context.Historico.FindAsync(id);
                _context.Historico.Remove(historico);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Nenhuma operação que está associado pode ser excluída!");
            }
        }

        // buscar por data
        public async Task<List<Historico>> BuscaPorData(DateTime? dataMin, DateTime? dataMax)
        {
            var resultado = from obj in _context.Historico select obj;

            if (dataMin.HasValue)
            {
                resultado = resultado.Where(x => x.DataOperacao >= dataMin.Value);
            }
            if (dataMax.HasValue)
            {
                resultado = resultado.Where(x => x.DataOperacao <= dataMax.Value);
            }

            return await resultado
                .Include(x => x.Papel)
                .OrderByDescending(x => x.DataOperacao)
                .ToListAsync();
        }
    }
}