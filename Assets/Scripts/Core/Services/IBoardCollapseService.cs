using System.Collections.Generic;
using MVP.Model;
using UnityEngine;

namespace Core.Services
{
    public interface IBoardCollapseService
    {
        CollapseResult Collapse(IBoardModel board, Dictionary<Vector2Int, ITilePresenter> presenters); 
    }
}