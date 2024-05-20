using UnityEngine;

// Điều khiển camera trong game bằng chuột

public class MouseLook : MonoBehaviour
{
    public static MouseLook instance;

    [Header("Settings")]
    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor = true;
    [Space]
    private Vector2 sensitivity = new Vector2(2, 2);
    [Space]
    public Vector2 smoothing = new Vector2(3, 3);

    [Header("First Person")]
    public GameObject characterBody;

    private Vector2 targetDirection;
    private Vector2 targetCharacterDirection;

    private Vector2 _mouseAbsolute;
    private Vector2 _smoothMouse;

    private Vector2 mouseDelta;

    /*[HideInInspector]
    public bool scoped;*/

    void Start()
    {
        instance = this;

        // Lưu hướng ban đầu của camera dưới dạng vector 2
        targetDirection = transform.localRotation.eulerAngles;

        // Lưu trữ hướng ban đầu của nhân vật nếu có dưới dạng vector 2
        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
        
        if (lockCursor)
            LockCursor();

    }

    // Khóa và ẩn con trỏ chuột ở giữa màn hình 
    public void LockCursor()
    {
        // make the cursor hidden and locked
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Chuyển từ hướng của camera và hướng nhân vật
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // Giá trị con chuột lấy ngay tại thời điểm hiện tại
        mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Thay đổi giá trị theo độ nhạy và độ mượt
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // sử dụng giá trị (a = 1f / smoothing.x và y) để làm mượt con trỏ chuột, 
            // nếu a = 1 thì góc quay camera sẽ thay đổi ngay lập tức khi di chuột
            // nếu a != 1 thì góc quay camera sẽ thay đổi từ từ khi di chuột 
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        // Cập nhật góc quay  
        _mouseAbsolute += _smoothMouse;

        // Giới hạn góc quay trong một khoảng cho trước
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        // Xoay camera theo trục y
        transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        // Xoay nhân vật 
        if (characterBody)
        {
            // Tạo góc quay quanh một trục  
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }
        else
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }
    }
}
