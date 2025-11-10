using System;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    public new static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField]
    private CuttingRecipeSO[] cutKitchenObjectSOArray;

    

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) {
            // There is no KitchenObject here
            if (player.HasKitchenObject()) {
                //Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    // Player carrying something that can be Cut
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    InteractLogicPlaceObjectOnCounterServerRpc();
                }
            }
            //Player not carrying anything
        }
        else {
            // There is a KitchenObject here
            if (player.HasKitchenObject()) {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
            }
            else {
                // Player is not carrying something
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progresssNormalized = 0f });
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc()
    {
        InteractLogicPlaceObjectOnCounterClientRpc();
    }

    [ClientRpc]
    private void InteractLogicPlaceObjectOnCounterClientRpc()
    {
        cuttingProgress = 0;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progresssNormalized = 0f});
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            CutObjectServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
        TestCuttingProgressDoneServerRpc();
    }

    [ClientRpc]
    private void CutObjectClientRpc()
    {
        // There is a KitchenObject here
        cuttingProgress++;
            
        OnCut?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progresssNormalized = (float) cuttingProgress / cuttingRecipeSO.cuttingProgressMax });
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void TestCuttingProgressDoneServerRpc()
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
        
        if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
            KitchenObjectSO outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

            KitchenObject.DestroyKitchenObject(GetKitchenObject());

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
        }
    }
    

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        }

        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cutKitchenObjectSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }

        return null;
    }

    
}
