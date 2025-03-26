using System.Collections.Generic;
using MVP.Model;
using UnityEngine;

namespace Core.Services
{
    public interface IMatchFinderService
    {
        List<Vector2Int> FindMatches(IBoardModel boardModel, Vector2Int position);
    }
}