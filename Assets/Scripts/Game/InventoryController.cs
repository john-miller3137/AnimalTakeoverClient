using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using Server.Game;
using SharedLibrary;
using SharedLibrary.Objects;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class InventoryController : MonoBehaviour
    {
        private static InventoryController instance;
        public static InventoryController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<InventoryController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("InventoryController");
                        instance = go.AddComponent<InventoryController>();
                    }
                }
                return instance;
            }
        }

        [SerializeField] private GameObject ice_seeds, poison_seeds, poppy_seeds, dandelion_seeds, marigold_seeds, 
            turret_seeds, shield_seeds;
        public GameObject inventory_select;

        private const float invStartX = -1.15f;
        private const float invStartY = -7.4f;
        private const float invSelectX = -1.45f;
        private const float invSelectY = -7.65f;

        public List<GameObject> inventoryItems;
        public List<GameItem> inventoryGameItems;
        public object inventoryLock, inventoryGUIlock;
        private int inventoryIndex = 0;
        private bool placedSeedFlag = false;

        private void Start()
        {
            inventoryGUIlock = new object();
            inventoryLock = new object();
            lock (inventoryLock)
            {
                inventoryItems = new List<GameObject>();
            }
            inventoryGameItems = new List<GameItem>();
        }

        public IEnumerator MoveObjectWithEasing(GameObject gameObject, Vector3 startPoint, Vector3 endPoint, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float normalizedTime = elapsedTime / duration;
                float easedTime = Easing.InOutCubic(normalizedTime);

                gameObject.transform.position = Vector3.Lerp(startPoint, endPoint, easedTime);

                elapsedTime += Time.deltaTime;
                yield return null; // Yield to allow the next frame
            }

            // Ensure that the object reaches the exact end position
            
            gameObject.transform.position = endPoint;
            StartCoroutine(ReformatSeeds());
        }

        public void AddItemToInventoryVoid(int itemId, int x, int y)
        {
            StartCoroutine(AddItemToInventory(itemId, x, y));
        }

        public IEnumerator AddItemToInventory(int itemId, int startX, int startY)
        {
            int invId = -1;
            GameObject s = null;
            GameObject s2 = null;
            lock (inventoryLock)
            {
                invId = inventoryItems.Count;
                Debug.Log("inventoryId" + invId + " itemId : " + itemId);

               
                if (invId >= 12 || invId < 0) yield break;
                Debug.Log(itemId);
                switch (itemId)
                {
                    case (int)ItemId.ICE_SEED:

                        Debug.Log("seeds initialize");
                        s = Instantiate(ice_seeds, new Vector3(startX, startY), Quaternion.identity);
                        inventoryItems.Add(s); 
                        GameItem sGI = new GameItem
                        {
                            ItemId = ItemId.ICE_SEED,
                            IsSeed = true,
                            Name = ConstantVars.ice_seed_name
                        };
                        inventoryGameItems.Add(sGI);


                        break;
                    case (int)ItemId.POISON_SEED:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.poison_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        
                        inventoryItems.Add(s);
                        GameItem ps = new GameItem
                        {
                            ItemId = ItemId.POISON_SEED,
                            Name = ConstantVars.poison_seed_name,
                            IsSeed = true
                        };
                        inventoryGameItems.Add(ps);
                       


                        break;
                    case (int)ItemId.POPPY_SEED:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.poppy_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        
                        inventoryItems.Add(s);
                        GameItem pops = new GameItem
                        {
                            ItemId = ItemId.POPPY_SEED,
                            Name = ConstantVars.poppy_seed_name,
                            IsSeed = true
                        };
                        inventoryGameItems.Add(pops);
                        


                        break;
                    case (int)ItemId.MARIGOLD_SEED:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.marigold_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        
                        inventoryItems.Add(s);
                        GameItem mGameItem = new GameItem
                        {
                            ItemId = ItemId.MARIGOLD_SEED,
                            Name = ConstantVars.marigold_seed_name,
                            IsSeed = true
                        };
                        inventoryGameItems.Add(mGameItem);
                        

                        break;
                    case (int)ItemId.TURRET_SEED:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.turret_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        inventoryItems.Add(s);
                        GameItem tGI = new GameItem
                        {
                            ItemId = ItemId.TURRET_SEED,
                            Name = ConstantVars.turret_seed_name,
                            IsSeed = true
                        };
                        inventoryGameItems.Add(tGI);
                        

                        break;
                    case (int)ItemId.DANDELION_SEED:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.dandelion_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        inventoryItems.Add(s);
                        GameItem dGI = new GameItem
                        {
                            ItemId = ItemId.DANDELION_SEED,
                            Name = ConstantVars.dandelion_seed_name,
                            IsSeed = true
                        };
                        inventoryGameItems.Add(dGI);
                        

                        break;
                    case (int)ItemId.ICE_SEED2:

                        Debug.Log("seeds initialize");
                        s = Instantiate(ice_seeds, new Vector3(startX, startY), Quaternion.identity);
                        inventoryItems.Add(s); 
                        s2 = Instantiate(ice_seeds, new Vector3(startX+0.5f, startY), Quaternion.identity);
                        inventoryItems.Add(s2);
                        GameItem sGI2 = new GameItem
                        {
                            ItemId = ItemId.ICE_SEED,
                            IsSeed = true,
                            Name = ConstantVars.ice_seed_name
                        };
                        inventoryGameItems.Add(sGI2);
                        inventoryGameItems.Add(sGI2);


                        break;
                    case (int)ItemId.POISON_SEED2w:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.poison_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        s2 = Instantiate(poison_seeds, new Vector3(startX+0.5f, startY), Quaternion.identity);
                        inventoryItems.Add(s);
                        inventoryItems.Add(s2);
                        GameItem ps2 = new GameItem
                        {
                            ItemId = ItemId.POISON_SEED,
                            Name = ConstantVars.poison_seed_name,
                            IsSeed = true
                        };
                        inventoryGameItems.Add(ps2);
                        inventoryGameItems.Add(ps2);


                        break;
                    case (int)ItemId.POPPY_SEED2:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.poppy_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        s2 = Instantiate(poppy_seeds, new Vector3(startX+0.5f, startY), Quaternion.identity);
                       
                        inventoryItems.Add(s);
                        inventoryItems.Add(s2);
                        GameItem pops2 = new GameItem
                        {
                            ItemId = ItemId.POPPY_SEED,
                            Name = ConstantVars.poppy_seed_name,
                            IsSeed = true
                        };
                        inventoryGameItems.Add(pops2);
                        inventoryGameItems.Add(pops2);


                        break;
                    case (int)ItemId.MARIGOLD_SEED2:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.marigold_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        s2 = Instantiate(marigold_seeds, new Vector3(startX+0.5f, startY), Quaternion.identity);
                        
                        inventoryItems.Add(s);
                        inventoryItems.Add(s2);
                        GameItem mGameItem2 = new GameItem
                        {
                            ItemId = ItemId.MARIGOLD_SEED,
                            Name = ConstantVars.marigold_seed_name,
                            IsSeed = true
                        };
                        inventoryGameItems.Add(mGameItem2);
                        inventoryGameItems.Add(mGameItem2);


                        break;
                    case (int)ItemId.TURRET_SEED2:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.turret_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        s2 = Instantiate(turret_seeds, new Vector3(startX+0.5f, startY), Quaternion.identity);
                        
                        inventoryItems.Add(s);
                        inventoryItems.Add(s2);
                        GameItem tGI2 = new GameItem
                        {
                            ItemId = ItemId.TURRET_SEED,
                            Name = ConstantVars.turret_seed_name,
                            IsSeed = true
                        };
                        inventoryGameItems.Add(tGI2);
                        inventoryGameItems.Add(tGI2);

                        break;
                    case (int)ItemId.DANDELION_SEED2:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.dandelion_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        s2 = Instantiate(this.dandelion_seeds, new Vector3(startX+0.5f, startY),
                            Quaternion.identity);
                        inventoryItems.Add(s);
                        inventoryItems.Add(s2);
                        GameItem dGI2 = new GameItem
                        {
                            ItemId = ItemId.DANDELION_SEED,
                            Name = ConstantVars.dandelion_seed_name,
                            IsSeed = true
                        };
                        inventoryGameItems.Add(dGI2);
                        inventoryGameItems.Add(dGI2);
                        break;
                    case (int)ItemId.SHIELD_SEED2:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.shield_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        s2 = Instantiate(this.shield_seeds, new Vector3(startX+0.5f, startY),
                            Quaternion.identity);
                        inventoryItems.Add(s);
                        inventoryItems.Add(s2);
                        GameItem sss2 = new GameItem
                        {
                            ItemId = ItemId.SHIELD_SEED,
                            Name = "Shield Seed",
                            IsSeed = true
                        };
                        inventoryGameItems.Add(sss2);
                        inventoryGameItems.Add(sss2);
                        break;
                    case (int)ItemId.SHIELD_SEED:
                        Debug.Log("seeds initialize");
                        s = Instantiate(this.shield_seeds, new Vector3(startX, startY),
                            Quaternion.identity);
                        inventoryItems.Add(s);
                        GameItem ss2 = new GameItem
                        {
                            ItemId = ItemId.SHIELD_SEED,
                            Name = "Shield Seed",
                            IsSeed = true
                        };
                        inventoryGameItems.Add(ss2);
                        break;
                }
            }

            bool s2Null = s2 == null;
            if (s != null)
            {
                BoxCollider2D bc = s.transform.GetChild(0).GetComponent<BoxCollider2D>();
                bc.enabled = false;
                if (!s2Null)
                {
                    s2.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
                }
                yield return new WaitForSeconds(2);
                bc.enabled = true;
                if (!s2Null)
                {
                    s2.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
                }
                lock (inventoryGUIlock)
                {
                    if (!s2Null)
                    {
                        StartCoroutine(MoveObjectWithEasing(s2, s2.transform.position, 
                            GetInventoryPosition(invId+1), 1));
                    }       
                    s.transform.GetChild(0).transform.position = new Vector3(0, -.2f, 0);
                    StartCoroutine(MoveObjectWithEasing(s, s.transform.position, 
                        GetInventoryPosition(invId), 1));
                }
            }

            yield return null;
        }

        private IEnumerator ReformatSeeds()
        {
            lock (inventoryLock)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    lock (inventoryGUIlock)
                    {
                        inventoryItems[i].transform.position = GetInventoryPosition(i);
                    }
                } 
            }
            
            
            yield return null;
        }
        public void DoRemoveItemFromInventory(int seedId)
        {
            StartCoroutine(RemoveItemFromInventory(seedId));
        }
        private IEnumerator RemoveItemFromInventory(int seedId)
        {
            lock (inventoryLock)
            {
                Debug.Log($"trying to remove {seedId} from inventory");
                int index = -1;
                for (int i = 0; i < inventoryGameItems.Count; i++)
                {
                    if ((int)inventoryGameItems[i].ItemId == seedId)
                    {
                        Debug.Log($"{seedId} seed removed at index " + i );
                        index = i;
                        lock (inventoryGUIlock)
                        {
                            Destroy(inventoryItems[i]);
                        }

                        inventoryItems.Remove(inventoryItems[i]);
                        inventoryGameItems.Remove(inventoryGameItems[i]);
                        break;
                    }
                }

                if (index > -1)
                {
                    for (int i = index; i < inventoryItems.Count; i++)
                    {
                       
                        lock (inventoryGUIlock)
                        {
                            inventoryItems[i].transform.position = GetInventoryPosition(i);
                        }
                    }
                }
            }

            yield return null;
        }

        private Vector3 GetInventoryPosition()
        {
            lock (inventoryLock)
            {
                return new Vector3((invStartX + (inventoryItems.Count % 3)*.9f), 
                    (invStartY - (inventoryItems.Count / 3)*.95f), 0);
            }
        }
        private Vector3 GetInventoryPosition(int i)
        {
            return new Vector3((invStartX + (i % 3)*.9f), 
                (invStartY - (i / 3)*.95f), 0);
        }
        public Vector3 GetInventorySelectPosition(int i)
        {
            if (i >= 12) i = 11;
            return new Vector3((invSelectX + (i % 3)*.9f), 
                (invSelectY - (i / 3)*.95f), 0);
        }
    }
}