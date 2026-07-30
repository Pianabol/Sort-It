using UnityEngine;

public class GameController : MonoBehaviour
{
    public BottleController FirstBottle;
    public BottleController SecondBottle;
    
    private Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main; 
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGame()) return;
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider == null) return;
        BottleController clickedBottle = hit.collider.GetComponent<BottleController>();
        if (clickedBottle == null) return;

        if (FirstBottle == null)
        {
            FirstBottle = clickedBottle;
            FirstBottle.SelectBottle(); 
            return;
        }

        if (FirstBottle == clickedBottle)
        {
            FirstBottle.DeselectBottle(); 
            FirstBottle = null;
            return;
        }

        SecondBottle = clickedBottle;
        FirstBottle.bottleControllerRef = SecondBottle;

        FirstBottle.UpdateTopColorValues();
        SecondBottle.UpdateTopColorValues();

        if (SecondBottle.FillBottleCheck(FirstBottle.topColor) == true)
        {
            FirstBottle.StartColorTransfer();
            // SKOR EKLEME SATIRI BURADAN SİLİNDİ! Artık sadece sıvıyı aktarıyor.
        }
        else
        {
            FirstBottle.DeselectBottle(); 
        }

        FirstBottle = null;
        SecondBottle = null;
    }
}