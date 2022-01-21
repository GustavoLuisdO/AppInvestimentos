using Investimentos.Data;
using Investimentos.Models;
using Investimentos.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investimentos.Services
{
    public class TipoPapelService
    {
        private readonly InvestimentosContext _context;

        public TipoPapelService(InvestimentosContext context)
        {
            _context = context;
        }

        // listar todos
        public async Task<List<TipoPapel>> BuscarTodos()
        {
            return await _context.TipoPapel.ToListAsync();
        }

        // buscar por Id
        public async Task<TipoPapel> BuscarId(int id)
        {
            return await _context.TipoPapel.FirstOrDefaultAsync(tipoPapel => tipoPapel.Id == id);
        }

        // inserir
        public async Task Inserir(TipoPapel tipoPapel)
        {
            try
            {
                _context.Add(tipoPapel);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // editar
        public async Task Editar(TipoPapel tipoPapel)
        {
            var obj = await _context.TipoPapel.AnyAsync(x => x.Id == tipoPapel.Id);
            if (!obj)
            {
                throw new NotFoundException("Tipo de Papel não encontrado!");
            }
            try
            {
                _context.Update(tipoPapel);
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
                var tipoPapel = await _context.TipoPapel.FindAsync(id);
                _context.TipoPapel.Remove(tipoPapel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Nenhum tipo de papel que está associado pode ser excluído!");
            }
        }
    }
}
