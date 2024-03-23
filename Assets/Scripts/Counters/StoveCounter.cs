using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    public event EventHandler<OnStageChangeEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStageChangeEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    {
                        fryingTimer += Time.deltaTime;

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                        });

                        if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                        {
                            // fried
                            GetKitchenObject().DestorySelf();
                            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                            state = State.Fried;
                            burningTimer = 0.0f;
                            burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                            OnStateChanged?.Invoke(this, new OnStageChangeEventArgs
                            {
                                state = state
                            });
                        }
                    }
                    break;
                case State.Fried:
                    {
                        burningTimer += Time.deltaTime;

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                        });

                        if (burningTimer > burningRecipeSO.burningTimerMax)
                        {
                            // burned
                            GetKitchenObject().DestorySelf();
                            KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                            state = State.Burned;

                            OnStateChanged?.Invoke(this, new OnStageChangeEventArgs
                            {
                                state = state
                            });

                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                            {
                                progressNormalized = 0.0f
                            });
                        }
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no KitchenOjbect

            if (player.HasKitchenObject())
            {
                // Player is carrying KitchenOjbect
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player carrying something that can be fried
                    player.GetKitchenObject().SetKitchenOjbectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0.0f;

                    OnStateChanged?.Invoke(this, new OnStageChangeEventArgs
                    {
                        state = state
                    });
                }
            }
            else
            {
                // Player not carrying anything
                
            }
        }
        else
        {
            // There is KitchenOjbect
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestorySelf();
                    }
                }

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStageChangeEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0.0f
                });
            }
            else
            {
                // Player retrieve KitchenObject
                GetKitchenObject().SetKitchenOjbectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStageChangeEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0.0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipSO in fryingRecipeSOArray)
        {
            if (fryingRecipSO.input == inputKitchenObjectSO)
            {
                return fryingRecipSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
