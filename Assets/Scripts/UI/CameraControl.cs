using PSG.IsleOfColors.Gameplay;
using UnityEngine;

namespace PSG.IsleOfColors.UI
{
    public class CameraControl : MonoBehaviour
    {
        #region serialized variables

        [SerializeField] Transform player1Transform;
        [SerializeField] Transform player2Transform;

        [Header("Camera pan settings")]
        [SerializeField]
        private float cameraPanSpeed;

        [SerializeField]
        private float distanceFromEdge;

        [SerializeField]
        private AnimationCurve xPositionBounds;

        [SerializeField]
        private AnimationCurve yPositionBounds;

        [Header("Zoom settings")]
        [SerializeField]
        private float orthoSizeChangeSpeed;

        [SerializeField]
        private AnimationCurve orthoSizeCurve;

        #endregion

        #region private variables

        private Transform activePlayer;
        private GameManager gameManager;
        private Camera mainCamera;

        private float startingZoom = 0;
        private Vector2 screenSize;
        private Vector2 cameraMovement;

        Vector3? previousMousePosition;

        private float currentZoom;

        Vector3 targetPosition;
        float targetOrthoSize;

        float boundsY;
        float boundsX;

        #endregion

        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);

            OnCurrentPlayerChanged(gameManager.Player1, gameManager.Player2);

            screenSize = new Vector2(Screen.width, Screen.height);
            mainCamera = Camera.main;
            currentZoom = startingZoom;

            UpdateCameraZoom();
        }

        private void OnCurrentPlayerChanged(Player currentPlayer, Player otherPlayer)
        {
            if (currentPlayer == gameManager.Player1)
                activePlayer = player1Transform;
            else
                activePlayer = player2Transform;
        }

        private void Update()
        {
            //transform.position = Vector3.Lerp(transform.position, activePlayer.position + Vector3.forward * transform.position.z, 1.5f * Time.deltaTime);
            SetCameraZoom();
            SetCameraMovement();
        }

        /// <summary>
        /// Sets camera movement variable based on controls.
        /// </summary>
        private void SetCameraMovement()
        {
            cameraMovement = Vector2.zero;
            var mousePosition = Input.mousePosition;

            // Save mouse drag position
            if (Input.GetMouseButtonDown(2))
                previousMousePosition = mousePosition;

            if (Input.GetMouseButtonUp(2))
            {
                previousMousePosition = null;
            }

            // Check mouse pan
            if (previousMousePosition != null)
            {
                cameraMovement = (previousMousePosition.Value - mousePosition).normalized * cameraPanSpeed * 6; // Hard coded multiplier
                previousMousePosition = mousePosition;
            }
            // Check screen edges
            else
            {
                if (mousePosition.x <= distanceFromEdge)
                    cameraMovement.x = -cameraPanSpeed;
                else if (mousePosition.x >= screenSize.x - distanceFromEdge)
                    cameraMovement.x = cameraPanSpeed;

                if (mousePosition.y <= distanceFromEdge - distanceFromEdge)
                    cameraMovement.y = -cameraPanSpeed;
                else if (mousePosition.y >= screenSize.y - distanceFromEdge)
                    cameraMovement.y = cameraPanSpeed;
            }

            // Apply bounds
            targetPosition = transform.position + new Vector3(cameraMovement.x, cameraMovement.y, 0);
            targetPosition.x = Mathf.Clamp(targetPosition.x, activePlayer.position.x - boundsX, activePlayer.position.x + boundsX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, activePlayer.position.y - boundsY, activePlayer.position.y + boundsY);
            
            // Apply movement, rotation and FOV
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraPanSpeed);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthoSize, Time.deltaTime * cameraPanSpeed);
        }


        private void SetCameraZoom()
        {
            var lastZoom = currentZoom;

            currentZoom = Mathf.Clamp(currentZoom + Input.GetAxis("Mouse ScrollWheel") * orthoSizeChangeSpeed * Time.deltaTime, 0, 1);

            if(lastZoom != currentZoom)
            {
                UpdateCameraZoom();
            }
        }

        private void UpdateCameraZoom()
        {
            boundsX = xPositionBounds.Evaluate(currentZoom);
            boundsY = yPositionBounds.Evaluate(currentZoom);

            targetOrthoSize = orthoSizeCurve.Evaluate(currentZoom);
        }
    }
}
