using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Runtime.InteropServices;

public class Purchase : MonoBehaviour, IStoreListener
{
	private static IStoreController m_StoreController;                                                                  // Reference to the Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider;                                                         // Reference to store-specific Purchasing subsystems.

	// Product identifiers for all products capable of being purchased: "convenience" general identifiers for use with Purchasing, and their store-specific identifier counterparts 
	// for use with and outside of Unity Purchasing. Define store-specific identifiers also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

	private static string kProductIDNonConsumable = "com.appmoss.fruitjumping.removeads";                                                  // General handle for the non-consumable product.

	private static string kProductNameAppleNonConsumable = "com.appmoss.fruitjumping.removeads";      // Apple App Store identifier for the non-consumable product.

	private static string kProductNameGooglePlayNonConsumable = "runnerfruitjumping.removeads";     // Google Play Store identifier for the non-consumable product.

	void Start()
	{
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null)
		{
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}
	}

	public void InitializePurchasing() 
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());


		builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable, new IDs(){{ kProductNameAppleNonConsumable,       AppleAppStore.Name },{ kProductNameGooglePlayNonConsumable,  GooglePlay.Name },});// And finish adding the subscription product.
		UnityPurchasing.Initialize(this, builder);
	}


	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	[DllImport ("__Internal")]
	private static extern void sendRemoveADS();

	public void BuyNonConsumable()
	{
		sendRemoveADS ();
		// Buy the non-consumable product using its general identifier. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
//		BuyProductID(kProductIDNonConsumable);
	}


	void BuyProductID(string productId)
	{
		foreach(var item in m_StoreController.products.all)
		{
			print (m_StoreController.products.all.Length);
		}
		// If the stores throw an unexpected exception, use try..catch to protect my logic here.
		try
		{
			// If Purchasing has been initialized ...
			if (IsInitialized())
			{
				// ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
				Product product = m_StoreController.products.WithID(productId);

				// If the look up found a product for this device's store and that product is ready to be sold ... 
				if (product != null && product.availableToPurchase)
				{
					Debug.Log (string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
					m_StoreController.InitiatePurchase(product);
				}
				// Otherwise ...
				else
				{
					// ... report the product look-up failure situation  
					Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			// Otherwise ...
			else
			{
				// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
				Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}
		// Complete the unexpected exception handling ...
		catch (Exception e)
		{
			// ... by reporting any unexpected exception for later diagnosis.
			Debug.Log ("BuyProductID: FAIL. Exception during purchase. " + e);
		}
	}


	[DllImport ("__Internal")]
	private static extern void sendRestorePurchases();

	// Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
	public void RestorePurchases()
	{
		sendRestorePurchases ();
//		// If Purchasing has not yet been set up ...
//		if (!IsInitialized())
//		{
//			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
//			Debug.Log("RestorePurchases FAIL. Not initialized.");
//			return;
//		}
//
//		// If we are running on an Apple device ... 
//		if (Application.platform == RuntimePlatform.IPhonePlayer || 
//			Application.platform == RuntimePlatform.OSXPlayer)
//		{
//			// ... begin restoring purchases
//			Debug.Log("RestorePurchases started ...");
//
//			// Fetch the Apple store-specific subsystem.
//			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
//			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
//			apple.RestoreTransactions((result) => {
//				// The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
//				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//			});
//		}
//		// Otherwise ...
//		else
//		{
//			// We are not running on an Apple device. No work is necessary to restore purchases.
//			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
//		}
	}


	//  
	// --- IStoreListener
	//

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
//		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}


	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
//		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
		// A consumable product has been purchased by this user.


		// Or ... a non-consumable product has been purchased by this user.
		if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
		{
			PlayerPrefs.SetString ("removeads", "yes");
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));// Or ... a subscription product has been purchased by this user.
		}
		else 
		{
			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));}// Return a flag indicating wither this product has completely been received, or if the application needs to be reminded of this purchase at next app launch. Is useful when saving purchased products to the cloud, and when that save is delayed.
		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",product.definition.storeSpecificId, failureReason));}
}
