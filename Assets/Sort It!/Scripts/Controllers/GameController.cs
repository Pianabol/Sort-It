using UnityEngine;

public class GameController : MonoBehaviour
{
    public BottleController FirstBottle;
    public BottleController SecondBottle;

    void Update()
    {
        // GÜVENLİK KİLİDİ: Sadece oyun içindeyken (GAME durumundayken) tıklamalara izin ver!
        if (GameManager.Instance != null && !GameManager.Instance.IsGame()) 
            return;

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if(hit.collider != null)
            {
                BottleController clickedBottle = hit.collider.GetComponent<BottleController>();

                if(clickedBottle != null)
                {
                    if(FirstBottle == null)
                    {
                        FirstBottle = clickedBottle;
                        FirstBottle.SelectBottle(); 
                    }
                    else 
                    {
                        if(FirstBottle == clickedBottle)
                        {
                            FirstBottle.DeselectBottle(); 
                            FirstBottle = null;
                        }
                        else
                        {
                            SecondBottle = clickedBottle;
                            FirstBottle.bottleControllerRef = SecondBottle;

                            FirstBottle.UpdateTopColorValues();
                            SecondBottle.UpdateTopColorValues();

                            if(SecondBottle.FillBottleCheck(FirstBottle.topColor) == true)
                            {
                                FirstBottle.StartColorTransfer();
                                
                                // SKOR EKLEME: Başarılı transferde hamleyi +1 yap
                                if (ScoreManager.Instance != null) ScoreManager.Instance.AddMove();
                                
                                FirstBottle = null;
                                SecondBottle = null;
                            }
                            else
                            {
                                FirstBottle.DeselectBottle(); 
                                FirstBottle = null;
                                SecondBottle = null;    
                            }
                        }
                    }
                }
            }
        }
    }
}