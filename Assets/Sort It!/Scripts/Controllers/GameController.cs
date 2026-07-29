using UnityEngine;

public class GameController : MonoBehaviour
{
    public BottleController FirstBottle;
    public BottleController SecondBottle;
    
    private Camera mainCam;

    void Awake()
    {
        // FPS Optimizasyonu: Her frame'de Camera.main çağırmak yerine başta hafızaya alıyoruz.
        mainCam = Camera.main; 
    }

    void Update()
    {
        // 1. GUARD CLAUSE: Oyun durumunda değilsek veya tıklama yoksa KODU AŞAĞI İNDİRME, ÇIK!
        if (GameManager.Instance == null || !GameManager.Instance.IsGame()) return;
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        // 2. GUARD CLAUSE: Boşa tıkladıysa veya tıkladığı şey şişe değilse ÇIK!
        if (hit.collider == null) return;
        BottleController clickedBottle = hit.collider.GetComponent<BottleController>();
        if (clickedBottle == null) return;

        // --- BUNDAN SONRASI TERTEMİZ OYUN MANTIĞI ---

        // Durum 1: Hiç şişe seçilmemişse
        if (FirstBottle == null)
        {
            FirstBottle = clickedBottle;
            FirstBottle.SelectBottle(); 
            return;
        }

        // Durum 2: Seçili şişeye tekrar tıklanmışsa (İptal et)
        if (FirstBottle == clickedBottle)
        {
            FirstBottle.DeselectBottle(); 
            FirstBottle = null;
            return;
        }

        // Durum 3: Hedef şişeye tıklanmışsa (Transferi dene)
        SecondBottle = clickedBottle;
        FirstBottle.bottleControllerRef = SecondBottle;

        FirstBottle.UpdateTopColorValues();
        SecondBottle.UpdateTopColorValues();

        if (SecondBottle.FillBottleCheck(FirstBottle.topColor) == true)
        {
            FirstBottle.StartColorTransfer();
            
            // Başarılı transferde skoru artır
            if (ScoreManager.Instance != null) ScoreManager.Instance.AddMove();
        }
        else
        {
            // Transfer geçersizse ilk şişeyi yerine indir
            FirstBottle.DeselectBottle(); 
        }

        // İşlem bitti, referansları temizle ki yeni hamleye hazır olsun
        FirstBottle = null;
        SecondBottle = null;
    }
}