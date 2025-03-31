using System;
using System.Threading.Tasks;
using UnityEngine;

public interface ITileView
{
    event Func<Task> OnTileClicked;
    void SetTileAppearance(Sprite sprite, Color color);
    Transform Transform { get; }
}
