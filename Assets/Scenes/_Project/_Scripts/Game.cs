using System;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private int _bombCount;
    [SerializeField] private CellFactory _cellFactory;
    [SerializeField] private Button _button;
    [SerializeField] private Image _endGamePanel;
    
    private MouseInput _mouseInput;
    private Player _player;
    private CellController _cellController;

    private void Start()
    {
        _mouseInput = new MouseInput();
        _button.onClick.AddListener(Restart);
        StartGame();
    }

    private void Restart()
    {
        _cellFactory.Clear();
        StartGame();
    }
    
    public void StopGame(bool victory)
    {
        _mouseInput.Disable();

        ShowPanel(victory);
    }

    private void ShowPanel(bool victory)
    {
        _endGamePanel.gameObject.SetActive(true);
        if (victory)
        {
            _endGamePanel.color = new Color(0f, 1f, 0f, 0.1f);
        }
        else
        {
            _endGamePanel.color = new Color(1f, 0f, 0f, 0.1f);
        }
    }

    private void StartGame()
    {
        _endGamePanel.gameObject.SetActive(false);
        _mouseInput.Enable();
        _player = new Player(_mouseInput, Camera.main);
        _cellController = new CellController(this,_cellFactory, _gridSize, _bombCount,_player);
    }
    
}