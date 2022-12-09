using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    private List<GameObject> _nodeList = new List<GameObject>();
    private List<Transform> nodePosition = new List<Transform>();
    private RectTransform _rectTransform;
    private bool generateButtonWasPressed = false;
    public GameObject NodePrefab;
    public Button generateButton;
    public Button shuffleButton;
    public float squareOffset = 5;
    public float topPosition = 5;
    public GameObject heightInput, widthInput;
    int height, width;

    // Start is called before the first frame update
    private void Start()
    {
        _rectTransform = this.GetComponent<RectTransform>();

        generateButton.onClick.AddListener(delegate() {GenerateButtonPressed();});
        shuffleButton.onClick.AddListener(delegate() {ShuffleButtonPressed();});

        
    }

    private void SetSquaresPosition()
    {
        var squareRect = _nodeList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = _nodeList[0].GetComponent<Transform>();

        var offset = new Vector2
        {
            x = (squareRect.width * squareTransform.localScale.x + squareOffset) * 0.01f,
            y = (squareRect.height * squareTransform.localScale.y + squareOffset) * 0.01f
        };

        var startPosition = GetFirstSquarePosition();
        int columnNumber = 0;
        int rowNumber = 0;
        foreach (var node in _nodeList)
        {
            if (rowNumber + 1 > height)
            {
                columnNumber ++;
                rowNumber = 0;
            }

            var positionX = startPosition.x + offset.x * columnNumber;
            var positionY = startPosition.y - offset.y * rowNumber;

            node.GetComponent<Transform>().position = new Vector2(positionX, positionY);
            nodePosition.Add(node.GetComponent<Transform>());
            rowNumber++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPosition = new Vector2 (0f, transform.position.y);
        var squareRect = _nodeList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = _nodeList[0].GetComponent<Transform>();
        var squareSize = new Vector2(0f, 0f);

        squareSize.x = squareRect.width * squareTransform.localScale.x;
        squareSize.y = squareRect.height * squareTransform.localScale.y;

        var midWidthPosition = (((width - 1) * squareSize.x) /2) * 0.01f;
        var midWidthHeight = (((height - 1) * squareSize.y) /2) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y += midWidthHeight;

        return startPosition;
    }

    private void CreateNodes()
    {
        var squareScale = GetSquareScale(new Vector3(5f, 5f, 0.1f));
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                _nodeList.Add(Instantiate(NodePrefab));
                _nodeList[_nodeList.Count -1].transform.SetParent(this.transform);
                _nodeList[_nodeList.Count - 1].GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);
                _nodeList[_nodeList.Count - 1].transform.localScale = squareScale;
            }

        }  
    }


    private void ShuffleList(List<Transform> list)
    {
        int n = list.Count;
        System.Random rand = new System.Random();

        for (int i = 0; i < n; i++)
        {
            Swap(list, i, i + rand.Next(n - i));
        }
    }
    private void Swap(List<Transform> list, int firstIndex, int secondIndex)
    {
        var temp = list[firstIndex].position;
        list[firstIndex].position = list[secondIndex].position;
        list[secondIndex].position = temp;
    }


    private void Update()
    {
        string heightFromInput = heightInput.GetComponent<TMP_InputField>().text;
        int.TryParse(heightFromInput, out height);
        string widthFromInput = widthInput.GetComponent<TMP_InputField>().text;
        int.TryParse(widthFromInput, out width);
    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = NodePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squareSize = new Vector2(0f, 0f);
        var startPosition = new Vector2(0f, 0f);

        squareSize.x = (squareRect.width * targetScale.x) + squareOffset;
        squareSize.y = (squareRect.height * targetScale.y) + squareOffset;

        var  midWidthPosition = ((width * squareSize.x) /2) * 0.01f;
        var midWidthHeight = ((height * squareSize.y) /2) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y = midWidthHeight;

        return startPosition.x < GetHalfScreenWidth() * -1 || startPosition.y > topPosition;
    }

    private float GetHalfScreenWidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;
        return width/2;
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjustment = 0.01f;

        while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;

            if (finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;
                return finalScale;
            }
        }

        return finalScale;
    }

    public void GenerateButtonPressed()
    {
        if (generateButtonWasPressed == false)
        {
            CreateNodes();
            SetSquaresPosition();
            generateButtonWasPressed = true;
        }
    }

    public void ShuffleButtonPressed()
    {
        ShuffleList(nodePosition);
    }
}
