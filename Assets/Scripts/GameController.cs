using UnityEngine;

public class GameController : MonoBehaviour
{

    public BottleController FirstBottle;
    public BottleController SecondBottle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if(hit.collider != null)
            {
                if(hit.collider.GetComponents<BottleController>()!= null)
                {
                    if(FirstBottle==null)
                    {
                        FirstBottle = hit.collider.GetComponent<BottleController>();
                    }
                    else 
                    {
                        if(FirstBottle == hit.collider.GetComponent<BottleController>())
                        {
                            FirstBottle = null;
                        }
                        else
                        {
                            SecondBottle = hit.collider.GetComponent<BottleController>();
                            FirstBottle.bottleControllerRef = SecondBottle;

                            FirstBottle.UpdateTopColorValues();
                            SecondBottle.UpdateTopColorValues();

                            if(SecondBottle.FillBottleCheck(FirstBottle.topColor) == true)
                            {
                                FirstBottle.StartColorTransfer();
                                FirstBottle = null;
                                SecondBottle = null;
                            }
                            else
                            {
                                FirstBottle = null;
                                SecondBottle = null;    
                            }

                        }

                    }

                }

            }

        }
    }
    */

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                BottleController clickedBottle = hit.collider.GetComponent<BottleController>();

                if (clickedBottle != null)
                {
                    // 1. Durum: Henüz hiç şişe seçilmediyse
                    if (FirstBottle == null)
                    {
                        FirstBottle = clickedBottle;
                        FirstBottle.SelectBottle(); // <-- Şişeyi havaya kaldır & büyüt!
                    }
                    else 
                    {
                        // 2. Durum: Aynı şişeye tekrar tıklandıysa (Seçimi İptal Et)
                        if (FirstBottle == clickedBottle)
                        {
                            FirstBottle.DeselectBottle(); // <-- Şişeyi yerine indir & küçült!
                            FirstBottle = null;
                        }
                        // 3. Durum: Farklı bir hedef şişeye tıklandıysa
                        else
                        {
                            SecondBottle = clickedBottle;
                            FirstBottle.bottleControllerRef = SecondBottle;

                            FirstBottle.UpdateTopColorValues();
                            SecondBottle.UpdateTopColorValues();

                            if (SecondBottle.FillBottleCheck(FirstBottle.topColor) == true)
                            {
                                // Transfer geçerli! Şişe zaten hedefe hareket edecek.
                                FirstBottle.StartColorTransfer();
                                FirstBottle = null;
                                SecondBottle = null;
                            }
                            else
                            {
                                // Transfer geçersiz! Seçilen ilk şişeyi yerine geri indir.
                                FirstBottle.DeselectBottle(); // <-- Yanlış hamlede yerine indir!
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
