﻿namespace Entities.Exceptions
{
    public sealed class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(int id) : base($"The Category with:{id} coluld not found.")
        {
        }
    }
}
