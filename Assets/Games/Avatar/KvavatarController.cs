using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//The controller for the Kvavatar, or KvaGram's avatar.
//There are currently 297 sprites of KvaGram. This can be hardcoded. it can.. Tried. Bad idea.
//An alternative that could be refactoring in later, would be a system of tags and Linq inquires to find the requested image.
//Current intended function: each time any of the settings are changed, a flag is set to update the sprite based on hardcoded logic.
//The update, when flagged, occours on next Update() tick. If settings are invalid, the sprite is not chaged. Ragardless if succesfull, flag is removed.

namespace Kvavatar
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class KvavatarController : MonoBehaviour
	{
		private SpriteRenderer sp;

		const int NUM_SPRITES = 297;
		const string LOC = "AvatarSprites/";




		static Sprite[] sprites;
		static void LoadSprites()
		{
			Sprites = new Sprite[NUM_SPRITES];
			for(int i = 0; i<NUM_SPRITES; i++)
			{
				Sprites[i] = Resources.Load<Sprite>(LOC+i);
			}
		}

		private bool needUpdate;

		[SerializeField]
		private Pose pose; // enum, default AtEase
		[SerializeField]
		private Expression expression; // enum, default Neutral
		[SerializeField]
		private Shades shades; // enum, default up
		[SerializeField]
		private Eyes eyes; //enum, default right
		[SerializeField]
		private bool talk; //bool, default false.
		[SerializeField]
		private int frame; //number, default 0. <- Used when more than one sprite is found. Used for variations and animations.

		public Pose Pose				{get{return pose;} 			set {pose = value; 			needUpdate = true;}}
		public Expression Expression	{get{return expression;}	set {expression = value;	needUpdate = true;}}
		public Shades Shades			{get{return shades;} 		set {shades = value;		needUpdate = true;}}
		public Eyes Eyes				{get{return eyes;} 			set {eyes = value; 			needUpdate = true;}}
		public bool Talk				{get{return talk;} 			set {talk = value; 			needUpdate = true;}}
		public int Frame				{get{return frame;} 		set {frame = value; 		needUpdate = true;}}

        public static Sprite[] Sprites
        {
            get
            {
				if(sprites == null)
					LoadSprites();
                return sprites;
            }

            set
            {
                sprites = value;
            }
        }

        public SpriteRenderer Sp
        {
            get
            {
				if(sp == null)
					sp = GetComponent<SpriteRenderer>();
                return sp;
            }

            set
            {
                sp = value;
            }
        }

        void Start()
		{
			Sp.sprite = Sprites[0];
		}


		void UpdateSprite()
		{
			List<SpriteEntry> spritelist;
			if(FindSprite(out spritelist))
			{
				int f = frame % spritelist.Count;
				int s = spritelist[f].id;
				Sp.sprite = Sprites[s];
				Debug.Log(String.Format("Found {0}, sel{1}: Id({2}) Pose({3}) Expr({4}) Shad({5}) Eye({6}) Talk({7})",
				spritelist.Count, f, s, spritelist[f].pose, spritelist[f].expression, spritelist[f].shades, spritelist[f].eyes, spritelist[f].talk));
			}
			else
			{
				Debug.Log("No sprites found!");
			}
			//if(s >= 0 && s < NUM_SPRITES)
			//	sp.sprite = sprites[s];s
			needUpdate = false;
		}
		private void OnValidate()
		{
			CorrectValues();

			UpdateSprite();
		}
		public void CorrectValues()
		{
			if(Shades == Shades.Down || Expression == Expression.Condescending || Expression == Expression.ManiacalLaugh || Expression == Expression.Dumbfonded)
				Eyes = Eyes.Closed;
			else if(Eyes == Eyes.Closed)
				Eyes = Eyes.Right;

			if(Pose == Pose.Cross)
				Expression = Expression.Condescending;
			else if(Pose == Pose.Maniacal)
				Expression = Expression.ManiacalLaugh;
			else if(Expression == Expression.ManiacalLaugh)
				Expression = Expression.Neutral;
		}


		//Cache
		[SerializeField]
		IEnumerable<SpriteEntry> poseList;
		[SerializeField]
		IEnumerable<SpriteEntry> exprList;
		[SerializeField]
		IEnumerable<SpriteEntry> ShadesList;


		bool FindSprite(out List<SpriteEntry> spritelist)
		{
			//These three lists might be saved as a cache.
			poseList = SPRITE_DATA.Where(s => s.pose == Pose);
			exprList = poseList.Where(s => s.expression == Expression);
			ShadesList = exprList.Where(s => s.shades == Shades);
			spritelist = ShadesList.ToList();


			spritelist = spritelist.Where(s => s.eyes == Eyes).ToList();
			if(Expression != Expression.Dumbfonded && Expression != Expression.ManiacalLaugh)
				spritelist = spritelist.Where(s => s.talk == Talk).ToList();
			if(spritelist.Count > 0)
				return true;


			return false;
		}
		public static readonly SpriteEntry[] SPRITE_DATA =
		{
			new SpriteEntry(0,		Pose.AtEase,   				Expression.Neutral,			Shades.Up,      Eyes.Right,     false),   	
			new SpriteEntry(1,		Pose.AtEase,   				Expression.Neutral,			Shades.Mid,     Eyes.Right,     false),   	
			new SpriteEntry(2,		Pose.AtEase,   				Expression.Neutral,			Shades.Down,    Eyes.Closed,   	false),   	
			new SpriteEntry(3,		Pose.AtEase,   				Expression.Neutral,			Shades.Down,    Eyes.Closed,	true),    	
			new SpriteEntry(4,		Pose.AtEase,   				Expression.Neutral,			Shades.Mid,     Eyes.Right,     true),    	
			new SpriteEntry(5,		Pose.AtEase,   				Expression.Neutral,			Shades.Up,      Eyes.Right,     true),    	
			new SpriteEntry(6,		Pose.AtEase,   				Expression.Dumbfonded,     	Shades.Down,    Eyes.Closed,	null),              			
			new SpriteEntry(7,		Pose.AtEase,   				Expression.Dumbfonded,     	Shades.Mid,	    Eyes.Closed,	null),              			
			new SpriteEntry(8,		Pose.AtEase,   				Expression.Dumbfonded,     	Shades.Up,	    Eyes.Closed,	null),              				
			new SpriteEntry(9,		Pose.SeeHere,  				Expression.Neutral,      	Shades.Up,     	Eyes.Right,     false),   	
			new SpriteEntry(10,		Pose.SeeHere,  				Expression.Neutral,      	Shades.Mid,     Eyes.Right,     false),   	
			new SpriteEntry(11,		Pose.SeeHere,  				Expression.Neutral,      	Shades.Down,    Eyes.Closed,  	false),   	
			new SpriteEntry(12,		Pose.SeeHere,  				Expression.Neutral,      	Shades.Down,    Eyes.Closed,  	true),    	
			new SpriteEntry(13,		Pose.SeeHere,  				Expression.Neutral,      	Shades.Mid,     Eyes.Right,     true),    	
			new SpriteEntry(14,		Pose.SeeHere,  				Expression.Neutral,      	Shades.Up,      Eyes.Right,    	true),    	
			new SpriteEntry(15,		Pose.SeeHere,  				Expression.Dumbfonded,     	Shades.Up,     	Eyes.Closed,	null),              				
			new SpriteEntry(16,		Pose.SeeHere,  				Expression.Dumbfonded,     	Shades.Mid,    	Eyes.Closed,	null),              				
			new SpriteEntry(17,		Pose.SeeHere,  				Expression.Dumbfonded,     	Shades.Down,   	Eyes.Closed,	null),              				
			new SpriteEntry(18,		Pose.Hello,       			Expression.Neutral,     	Shades.Down,   	Eyes.Closed,    false),  	
			new SpriteEntry(19,		Pose.Hello,       			Expression.Neutral,     	Shades.Mid,    	Eyes.Right,     false),   	
			new SpriteEntry(20,		Pose.Hello,       			Expression.Neutral,     	Shades.Up,     	Eyes.Right,     false),   	
			new SpriteEntry(21,		Pose.Hello,       			Expression.Neutral,     	Shades.Up,   	Eyes.Right,     true),    	
			new SpriteEntry(22,		Pose.Hello,       			Expression.Neutral,     	Shades.Mid,  	Eyes.Right,     true),    	
			new SpriteEntry(23,		Pose.Hello,       			Expression.Neutral,     	Shades.Down, 	Eyes.Closed,	true),	
			new SpriteEntry(24,		Pose.Hello,       			Expression.Dumbfonded,     	Shades.Down,   	Eyes.Closed,	null),              				
			new SpriteEntry(25,		Pose.Hello,       			Expression.Dumbfonded,     	Shades.Mid,    	Eyes.Closed,	null),              				
			new SpriteEntry(26,		Pose.Hello,       			Expression.Dumbfonded,     	Shades.Up,     	Eyes.Closed,	null),             				
			new SpriteEntry(27,		Pose.LaptopWatching,     	Expression.Neutral,     	Shades.Up,    	Eyes.Right,     false),   	
			new SpriteEntry(28,		Pose.LaptopWatching,     	Expression.Neutral,     	Shades.Mid,   	Eyes.Right,     false),   	
			new SpriteEntry(29,		Pose.LaptopWatching,     	Expression.Neutral,     	Shades.Down,  	Eyes.Closed,  	false),   	
			new SpriteEntry(30,		Pose.LaptopWatching,     	Expression.Neutral,     	Shades.Down,  	Eyes.Closed,  	true),   	
			new SpriteEntry(31,		Pose.LaptopWatching,     	Expression.Neutral,     	Shades.Mid,  	Eyes.Right,     true),   	
			new SpriteEntry(32,		Pose.LaptopWatching,     	Expression.Neutral,     	Shades.Up,     	Eyes.Right,     true),  	
			new SpriteEntry(33,		Pose.LaptopWatching,     	Expression.Dumbfonded,     	Shades.Up,     	Eyes.Closed,	null),              				
			new SpriteEntry(34,		Pose.LaptopWatching,     	Expression.Dumbfonded,     	Shades.Mid,    	Eyes.Closed,	null),              				
			new SpriteEntry(35,		Pose.LaptopWatching,     	Expression.Dumbfonded,     	Shades.Down,   	Eyes.Closed,	null),              				
			new SpriteEntry(36,		Pose.LaptopPointing,     	Expression.Neutral,     	Shades.Down,   	Eyes.Closed,    false),  	
			new SpriteEntry(37,		Pose.LaptopPointing,     	Expression.Neutral,     	Shades.Mid,    	Eyes.Right,     false),  	
			new SpriteEntry(38,		Pose.LaptopPointing,     	Expression.Neutral,     	Shades.Up,    	Eyes.Right,     false),   	
			new SpriteEntry(39,		Pose.LaptopPointing,     	Expression.Neutral,     	Shades.Up,    	Eyes.Right,     true),   	
			new SpriteEntry(40,		Pose.LaptopPointing,     	Expression.Neutral,     	Shades.Mid,   	Eyes.Right,     true),   	
			new SpriteEntry(41,		Pose.LaptopPointing,     	Expression.Neutral,     	Shades.Down,  	Eyes.Closed,	true),   	
			new SpriteEntry(42,		Pose.LaptopPointing,     	Expression.Dumbfonded,  	Shades.Down,    Eyes.Closed,	null),              				
			new SpriteEntry(43,		Pose.LaptopPointing,     	Expression.Dumbfonded,  	Shades.Mid,     Eyes.Closed,	null),              				
			new SpriteEntry(44,		Pose.LaptopPointing,     	Expression.Dumbfonded,  	Shades.Up,      Eyes.Closed,	null),           				
			new SpriteEntry(45,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Up,      Eyes.Right,     false),   	
			new SpriteEntry(46,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Mid,     Eyes.Right,     false),   	
			new SpriteEntry(47,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Down,    Eyes.Closed,	false),   	
			new SpriteEntry(48,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Down,    Eyes.Closed,	true),   	
			new SpriteEntry(49,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Mid,     Eyes.Right,     true),   	
			new SpriteEntry(50,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Up,     	Eyes.Right,     true),   	
			new SpriteEntry(51,		Pose.LaptopTyping,     		Expression.Dumbfonded,     	Shades.Up,      Eyes.Closed, 	null),             				
			new SpriteEntry(52,		Pose.LaptopTyping,     		Expression.Dumbfonded,     	Shades.Mid,     Eyes.Closed, 	null),             			
			new SpriteEntry(53,		Pose.LaptopTyping,     		Expression.Dumbfonded,     	Shades.Down,    Eyes.Closed,	null),
			
			new SpriteEntry(54,		Pose.AtEase,    			Expression.Neutral,     	Shades.Mid,   	Eyes.Left,     	false),
			new SpriteEntry(55,		Pose.AtEase,    			Expression.Neutral,     	Shades.Up,    	Eyes.Left,     	false),
			new SpriteEntry(56,		Pose.AtEase,    			Expression.Neutral,     	Shades.Up,    	Eyes.Left,     	true),
			new SpriteEntry(57,		Pose.AtEase,    			Expression.Neutral,     	Shades.Mid,   	Eyes.Left,     	true),
			new SpriteEntry(58,		Pose.SeeHere,   			Expression.Neutral,     	Shades.Mid,   	Eyes.Left,     	false),
			new SpriteEntry(59,		Pose.SeeHere,   			Expression.Neutral,     	Shades.Up,    	Eyes.Left,     	false),
			new SpriteEntry(60,		Pose.SeeHere,   			Expression.Neutral,     	Shades.Up,    	Eyes.Left,     	true),
			new SpriteEntry(61,		Pose.SeeHere,   			Expression.Neutral,     	Shades.Mid,   	Eyes.Left,     	true),
			new SpriteEntry(62,		Pose.Hello,     			Expression.Neutral,     	Shades.Mid,   	Eyes.Left,     	true),
			new SpriteEntry(63,		Pose.Hello,     			Expression.Neutral,     	Shades.Up,    	Eyes.Left,     	true),
			new SpriteEntry(64,		Pose.LaptopWatching,    	Expression.Neutral,     	Shades.Up,    	Eyes.Left,     	false),
			new SpriteEntry(65,		Pose.LaptopWatching,    	Expression.Neutral,     	Shades.Mid,   	Eyes.Left,     	false),
			new SpriteEntry(66,		Pose.LaptopWatching,    	Expression.Neutral,     	Shades.Mid,   	Eyes.Left,     	true),
			new SpriteEntry(67,		Pose.LaptopWatching,    	Expression.Neutral,     	Shades.Up,    	Eyes.Left,     	true),
			new SpriteEntry(68,		Pose.LaptopPointing,    	Expression.Neutral,     	Shades.Up,    	Eyes.Left,     	false),
			new SpriteEntry(69,		Pose.LaptopPointing,    	Expression.Neutral,     	Shades.Mid,   	Eyes.Left,     	false),
			new SpriteEntry(70,		Pose.LaptopPointing,    	Expression.Neutral,     	Shades.Mid,   	Eyes.Left,     	true),
			new SpriteEntry(71,		Pose.LaptopPointing,    	Expression.Neutral,     	Shades.Up,    	Eyes.Left,     	true),
			new SpriteEntry(72,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Up,    	Eyes.Left,     	false),
			new SpriteEntry(73,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Mid,   	Eyes.Left,     	false),
			new SpriteEntry(74,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Mid,   	Eyes.Left,     	true),
			new SpriteEntry(75,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Up,    	Eyes.Left,     	true),
			new SpriteEntry(76,		Pose.AtEase, 				Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	false),
			new SpriteEntry(77,		Pose.AtEase, 				Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	false),
			new SpriteEntry(78,		Pose.AtEase, 				Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	true),
			new SpriteEntry(79,		Pose.AtEase, 				Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	true),
			new SpriteEntry(80,		Pose.SeeHere,				Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	false),
			new SpriteEntry(81,		Pose.SeeHere,				Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	false),
			new SpriteEntry(82,		Pose.SeeHere,				Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	true),
			new SpriteEntry(83,		Pose.SeeHere,				Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	true),
			new SpriteEntry(84,		Pose.Hello,					Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	false),
			new SpriteEntry(85,		Pose.Hello,					Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	false),
			new SpriteEntry(86,		Pose.Hello,					Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	true),
			new SpriteEntry(87,		Pose.Hello,					Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	true),
			new SpriteEntry(88,		Pose.LaptopWatching,   		Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	false),
			new SpriteEntry(89,		Pose.LaptopWatching,   		Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	false),
			new SpriteEntry(90,		Pose.LaptopWatching,   		Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	true),
			new SpriteEntry(91,		Pose.LaptopWatching,   		Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	true),
			new SpriteEntry(92,		Pose.LaptopPointing,   		Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	false),
			new SpriteEntry(93,		Pose.LaptopPointing,   		Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	false),
			new SpriteEntry(94,		Pose.LaptopPointing,   		Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	true),
			new SpriteEntry(95,		Pose.LaptopPointing,   		Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	true),
			new SpriteEntry(96,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	false),
			new SpriteEntry(97,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	false),
			new SpriteEntry(98,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Mid,   	Eyes.Down,     	true),
			new SpriteEntry(99,		Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Up,    	Eyes.Down,     	true),

			new SpriteEntry(100,	Pose.AtEase, 				Expression.Neutral,     	Shades.Up,   	Eyes.Up,       false),
			new SpriteEntry(101,	Pose.AtEase, 				Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       false),
			new SpriteEntry(102,	Pose.AtEase, 				Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       true),
			new SpriteEntry(103,	Pose.AtEase, 				Expression.Neutral,     	Shades.Up,   	Eyes.Up,       true),
			new SpriteEntry(104,	Pose.SeeHere,				Expression.Neutral,     	Shades.Up,   	Eyes.Up,       false),
			new SpriteEntry(105,	Pose.SeeHere,				Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       false),
			new SpriteEntry(106,	Pose.SeeHere,				Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       true),
			new SpriteEntry(107,	Pose.SeeHere,				Expression.Neutral,     	Shades.Up,   	Eyes.Up,       true),
			new SpriteEntry(108,	Pose.Hello,					Expression.Neutral,     	Shades.Up,   	Eyes.Up,       false),
			new SpriteEntry(109,	Pose.Hello,					Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       false),
			new SpriteEntry(110,	Pose.Hello,					Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       true),
			new SpriteEntry(111,	Pose.Hello,					Expression.Neutral,     	Shades.Up,   	Eyes.Up,       true),
			new SpriteEntry(112,	Pose.LaptopWatching,   		Expression.Neutral,     	Shades.Up,   	Eyes.Up,       false),
			new SpriteEntry(113,	Pose.LaptopWatching,   		Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       false),
			new SpriteEntry(114,	Pose.LaptopWatching,   		Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       true),
			new SpriteEntry(115,	Pose.LaptopWatching,   		Expression.Neutral,     	Shades.Up,   	Eyes.Up,       true),
			new SpriteEntry(116,	Pose.LaptopPointing,   		Expression.Neutral,     	Shades.Up,   	Eyes.Up,       false),
			new SpriteEntry(117,	Pose.LaptopPointing,   		Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       false),
			new SpriteEntry(118,	Pose.LaptopPointing,   		Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       true),
			new SpriteEntry(119,	Pose.LaptopPointing,   		Expression.Neutral,     	Shades.Up,   	Eyes.Up,       true),
			new SpriteEntry(120,	Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Up,   	Eyes.Up,       false),
			new SpriteEntry(121,	Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       false),
			new SpriteEntry(122,	Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Mid,  	Eyes.Up,       true),
			new SpriteEntry(123,	Pose.LaptopTyping,     		Expression.Neutral,     	Shades.Up,   	Eyes.Up,       true),

			new SpriteEntry(124,	Pose.Maniacal,     			Expression.ManiacalLaugh,   Shades.Up,		Eyes.Closed,	null), //opaque glasses
			new SpriteEntry(125,	Pose.Maniacal,     			Expression.ManiacalLaugh,   Shades.Mid,		Eyes.Closed,	null),//opaque glasses
			new SpriteEntry(126,	Pose.Maniacal,     			Expression.ManiacalLaugh,   Shades.Down,	Eyes.Closed,	null),
			new SpriteEntry(127,	Pose.Maniacal,     			Expression.ManiacalLaugh,   Shades.Up,		Eyes.Closed,	null), //Clear glasses
			new SpriteEntry(128,	Pose.Maniacal,     			Expression.ManiacalLaugh,   Shades.Mid,		Eyes.Closed,	null),//Clear glasses

			new SpriteEntry(129,	Pose.Cross,     Expression.Condescending,     Shades.Up,	Eyes.Closed,	false),
			new SpriteEntry(130,	Pose.Cross,     Expression.Condescending,     Shades.Mid,   Eyes.Closed,	false),
			new SpriteEntry(131,	Pose.Cross,     Expression.Condescending,     Shades.Down,  Eyes.Closed,	false),
			new SpriteEntry(132,	Pose.Cross,     Expression.Condescending,     Shades.Down,  Eyes.Closed,	true),
			new SpriteEntry(133,	Pose.Cross,     Expression.Condescending,     Shades.Mid,   Eyes.Closed,	true),
			new SpriteEntry(134,	Pose.Cross,     Expression.Condescending,     Shades.Up,  	Eyes.Closed,   true),

			new SpriteEntry(135,	Pose.AtEase,     Expression.Happy,     Shades.Up,   Eyes.Right,			false),
			new SpriteEntry(136,	Pose.AtEase,     Expression.Happy,     Shades.Mid,  Eyes.Right,     	false),
			new SpriteEntry(137,	Pose.AtEase,     Expression.Happy,     Shades.Down, Eyes.Closed,		false),
			new SpriteEntry(138,	Pose.SeeHere,     Expression.Happy,     Shades.Down, Eyes.Closed, 		false),
			new SpriteEntry(139,	Pose.SeeHere,     Expression.Happy,     Shades.Mid,  Eyes.Right,    	false),
			new SpriteEntry(140,	Pose.SeeHere,     Expression.Happy,     Shades.Up,   Eyes.Right,    	false),
			new SpriteEntry(141,	Pose.Hello,     Expression.Happy,     Shades.Up,   Eyes.Right,      	false),
			new SpriteEntry(142,	Pose.Hello,     Expression.Happy,     Shades.Mid,  Eyes.Right,      	false),
			new SpriteEntry(143,	Pose.Hello,     Expression.Happy,     Shades.Down, Eyes.Closed,			false),
			new SpriteEntry(144,	Pose.Hello,     Expression.Happy,     Shades.Down, Eyes.Closed,			true),
			new SpriteEntry(145,	Pose.Hello,     Expression.Happy,     Shades.Mid,  Eyes.Right,      	true),
			new SpriteEntry(146,	Pose.Hello,     Expression.Happy,     Shades.Up,   Eyes.Right,      	true),
			new SpriteEntry(147,	Pose.SeeHere,     Expression.Happy,     Shades.Up,   Eyes.Right,    	true),
			new SpriteEntry(148,	Pose.SeeHere,     Expression.Happy,     Shades.Mid,  Eyes.Right,    	true),
			new SpriteEntry(149,	Pose.SeeHere,     Expression.Happy,     Shades.Down, Eyes.Closed,		true),
			new SpriteEntry(150,	Pose.AtEase,     Expression.Happy,     Shades.Down, Eyes.Closed,		true),
			new SpriteEntry(151,	Pose.AtEase,     Expression.Happy,     Shades.Mid,  Eyes.Right,     	true),
			new SpriteEntry(152,	Pose.AtEase,     Expression.Happy,     Shades.Up,   Eyes.Right,     	true),
			new SpriteEntry(153,	Pose.LaptopWatching,    Expression.Happy,     Shades.Up,   Eyes.Right,  false),
			new SpriteEntry(154,	Pose.LaptopWatching,    Expression.Happy,     Shades.Mid,  Eyes.Right,  false),
			new SpriteEntry(155,	Pose.LaptopWatching,    Expression.Happy,     Shades.Down, Eyes.Closed,	false),
			new SpriteEntry(156,	Pose.LaptopWatching,    Expression.Happy,     Shades.Up,   Eyes.Right,  true),
			new SpriteEntry(157,	Pose.LaptopWatching,    Expression.Happy,     Shades.Mid,  Eyes.Right,  true),
			new SpriteEntry(158,	Pose.LaptopWatching,    Expression.Happy,     Shades.Down, Eyes.Closed,	true),
			new SpriteEntry(159,	Pose.LaptopPointing,    Expression.Happy,     Shades.Down, Eyes.Closed,	false),
			new SpriteEntry(160,	Pose.LaptopPointing,    Expression.Happy,     Shades.Mid,  Eyes.Right,  false),
			new SpriteEntry(161,	Pose.LaptopPointing,    Expression.Happy,     Shades.Up,   Eyes.Right,  false),
			new SpriteEntry(162,	Pose.LaptopPointing,    Expression.Happy,     Shades.Up,   Eyes.Right,  true),
			new SpriteEntry(163,	Pose.LaptopPointing,    Expression.Happy,     Shades.Mid,  Eyes.Right,  true),
			new SpriteEntry(164,	Pose.LaptopPointing,    Expression.Happy,     Shades.Down, Eyes.Closed,	true),
			new SpriteEntry(165,	Pose.LaptopTyping,     	Expression.Happy,     Shades.Down, Eyes.Closed,	true),
			new SpriteEntry(166,	Pose.LaptopTyping,     	Expression.Happy,     Shades.Mid,  Eyes.Right,  true),
			new SpriteEntry(167,	Pose.LaptopTyping,     	Expression.Happy,     Shades.Up,   Eyes.Right,  true),
			new SpriteEntry(168,	Pose.LaptopTyping,     	Expression.Happy,     Shades.Up,   Eyes.Right,  false),
			new SpriteEntry(169,	Pose.LaptopTyping,     	Expression.Happy,     Shades.Mid,  Eyes.Right,  false),
			new SpriteEntry(170,	Pose.LaptopTyping,     	Expression.Happy,     Shades.Down, Eyes.Closed,	false),
			new SpriteEntry(171,	Pose.Ecstatic,     		Expression.Happy,     Shades.Down, Eyes.Closed,	false),
			new SpriteEntry(172,	Pose.Ecstatic,     		Expression.Happy,     Shades.Mid,  Eyes.Right,  false),
			new SpriteEntry(173,	Pose.Ecstatic,     		Expression.Happy,     Shades.Up,   Eyes.Right,  false),
			new SpriteEntry(174,	Pose.Ecstatic,     		Expression.Happy,     Shades.Up,   Eyes.Right,  true),
			new SpriteEntry(175,	Pose.Ecstatic,     		Expression.Happy,     Shades.Mid,  Eyes.Right,  true),
			new SpriteEntry(176,	Pose.Ecstatic,     		Expression.Happy,     Shades.Down, Eyes.Closed, true),
			new SpriteEntry(177,	Pose.Ecstatic,     		Expression.Happy,     Shades.Mid,  Eyes.Right,  true),
			new SpriteEntry(178,	Pose.Ecstatic,     		Expression.Happy,     Shades.Up,   Eyes.Right,  true),
			new SpriteEntry(179,	Pose.Ecstatic,     		Expression.Happy,     Shades.Up,   Eyes.Right,  false),
			new SpriteEntry(180,	Pose.Ecstatic,     		Expression.Happy,     Shades.Mid,  Eyes.Right,  false),
			
			new SpriteEntry(181,	Pose.Ecstatic,		Expression.Happy,     Shades.Up,   Eyes.Left,     false),
			new SpriteEntry(182,	Pose.Ecstatic,		Expression.Happy,     Shades.Mid,  Eyes.Left,     false),
			new SpriteEntry(183,	Pose.Ecstatic,		Expression.Happy,     Shades.Mid,  Eyes.Left,     true),
			new SpriteEntry(184,	Pose.Ecstatic,		Expression.Happy,     Shades.Up,   Eyes.Left,     true),
			new SpriteEntry(185,	Pose.AtEase,    	Expression.Happy,     Shades.Up,   Eyes.Left,     false),
			new SpriteEntry(186,	Pose.AtEase,    	Expression.Happy,     Shades.Mid,  Eyes.Left,     false),
			new SpriteEntry(187,	Pose.AtEase,    	Expression.Happy,     Shades.Mid,  Eyes.Left,     true),
			new SpriteEntry(188,	Pose.AtEase,    	Expression.Happy,     Shades.Up,   Eyes.Left,     true),
			new SpriteEntry(189,	Pose.SeeHere,   	Expression.Happy,     Shades.Up,   Eyes.Left,     false),
			new SpriteEntry(190,	Pose.SeeHere,   	Expression.Happy,     Shades.Mid,  Eyes.Left,     false),
			new SpriteEntry(191,	Pose.SeeHere,   	Expression.Happy,     Shades.Mid,  Eyes.Left,     true),
			new SpriteEntry(192,	Pose.SeeHere,   	Expression.Happy,     Shades.Up,   Eyes.Left,     true),
			new SpriteEntry(193,	Pose.Hello,     	Expression.Happy,     Shades.Up,   Eyes.Left,     true),
			new SpriteEntry(194,	Pose.Hello,     	Expression.Happy,     Shades.Mid,  Eyes.Left,     true),
			new SpriteEntry(195,	Pose.Hello,     	Expression.Happy,     Shades.Mid,  Eyes.Left,     false),
			new SpriteEntry(196,	Pose.Hello,     	Expression.Happy,     Shades.Up,   Eyes.Left,     false),
			new SpriteEntry(197,	Pose.LaptopTyping,  Expression.Happy,     Shades.Up,   Eyes.Left,     false),
			new SpriteEntry(198,	Pose.LaptopTyping,  Expression.Happy,     Shades.Mid,  Eyes.Left,     false),
			new SpriteEntry(199,	Pose.LaptopTyping,  Expression.Happy,     Shades.Mid,  Eyes.Left,     true),
			new SpriteEntry(200,	Pose.LaptopTyping,  Expression.Happy,     Shades.Up,   Eyes.Left,     true),
			new SpriteEntry(201,	Pose.LaptopWatching,Expression.Happy,     Shades.Up,   Eyes.Left,     true),
			new SpriteEntry(202,	Pose.LaptopWatching,Expression.Happy,     Shades.Mid,  Eyes.Left,     true),
			new SpriteEntry(203,	Pose.LaptopWatching,Expression.Happy,     Shades.Mid,  Eyes.Left,     false),
			new SpriteEntry(204,	Pose.LaptopWatching,Expression.Happy,     Shades.Up,   Eyes.Left,     false),
			new SpriteEntry(205,	Pose.LaptopPointing,Expression.Happy,     Shades.Up,   Eyes.Left,     false),
			new SpriteEntry(206,	Pose.LaptopPointing,Expression.Happy,     Shades.Mid,  Eyes.Left,     false),
			new SpriteEntry(207,	Pose.LaptopPointing,Expression.Happy,     Shades.Mid,  Eyes.Left,     true),
			new SpriteEntry(208,	Pose.LaptopPointing,Expression.Happy,     Shades.Up,   Eyes.Left,     true),

			new SpriteEntry(209,	Pose.LaptopPointing,Expression.Happy,     Shades.Up,   Eyes.Up,     true),
			new SpriteEntry(210,	Pose.LaptopPointing,Expression.Happy,     Shades.Mid,  Eyes.Up,     true),
			new SpriteEntry(211,	Pose.LaptopPointing,Expression.Happy,     Shades.Mid,  Eyes.Up,     false),
			new SpriteEntry(212,	Pose.LaptopPointing,Expression.Happy,     Shades.Up,   Eyes.Up,     false),
			new SpriteEntry(213,	Pose.LaptopWatching,Expression.Happy,     Shades.Up,   Eyes.Up,     false),
			new SpriteEntry(214,	Pose.LaptopWatching,Expression.Happy,     Shades.Mid,  Eyes.Up,     false),
			new SpriteEntry(215,	Pose.LaptopWatching,Expression.Happy,     Shades.Mid,  Eyes.Up,     true),
			new SpriteEntry(216,	Pose.LaptopWatching,Expression.Happy,     Shades.Up,   Eyes.Up,     true),
			new SpriteEntry(217,	Pose.LaptopTyping,  Expression.Happy,     Shades.Up,   Eyes.Up,     true),
			new SpriteEntry(218,	Pose.LaptopTyping,  Expression.Happy,     Shades.Mid,  Eyes.Up,     true),
			new SpriteEntry(219,	Pose.LaptopTyping,  Expression.Happy,     Shades.Mid,  Eyes.Up,     false),
			new SpriteEntry(220,	Pose.LaptopTyping,  Expression.Happy,     Shades.Up,   Eyes.Up,     false),
			new SpriteEntry(221,	Pose.Ecstatic,      Expression.Happy,     Shades.Up,   Eyes.Up,     false),
			new SpriteEntry(222,	Pose.Ecstatic,      Expression.Happy,     Shades.Mid,  Eyes.Up,     false),
			new SpriteEntry(223,	Pose.Ecstatic,      Expression.Happy,     Shades.Mid,  Eyes.Up,     true),
			new SpriteEntry(224,	Pose.Ecstatic,      Expression.Happy,     Shades.Up,   Eyes.Up,     true),
			new SpriteEntry(225,	Pose.AtEase,        Expression.Happy,     Shades.Up,   Eyes.Up,     true),
			new SpriteEntry(226,	Pose.AtEase,        Expression.Happy,     Shades.Mid,  Eyes.Up,     true),
			new SpriteEntry(227,	Pose.AtEase,        Expression.Happy,     Shades.Mid,  Eyes.Up,     false),
			new SpriteEntry(228,	Pose.AtEase,        Expression.Happy,     Shades.Up,   Eyes.Up,     false),
			new SpriteEntry(229,	Pose.SeeHere,     	Expression.Happy,     Shades.Up,   Eyes.Up,     false),
			new SpriteEntry(230,	Pose.SeeHere,     	Expression.Happy,     Shades.Mid,  Eyes.Up,     false),
			new SpriteEntry(231,	Pose.SeeHere,     	Expression.Happy,     Shades.Mid,  Eyes.Up,     true),
			new SpriteEntry(232,	Pose.SeeHere,     	Expression.Happy,     Shades.Up,   Eyes.Up,     true),
			new SpriteEntry(233,	Pose.Hello,	     	Expression.Happy,     Shades.Up,   Eyes.Up,     true),
			new SpriteEntry(234,	Pose.Hello,	     	Expression.Happy,     Shades.Mid,  Eyes.Up,     true),
			new SpriteEntry(235,	Pose.Hello,	     	Expression.Happy,     Shades.Mid,  Eyes.Up,     false),
			new SpriteEntry(236,	Pose.Hello,	     	Expression.Happy,     Shades.Up,   Eyes.Up,     false),
			
			new SpriteEntry(237,	Pose.Hello,     	Expression.Happy,     Shades.Up,   Eyes.Down,     false),
			new SpriteEntry(238,	Pose.Hello,     	Expression.Happy,     Shades.Mid,  Eyes.Down,     false),
			new SpriteEntry(239,	Pose.Hello,     	Expression.Happy,     Shades.Mid,  Eyes.Down,     true),
			new SpriteEntry(240,	Pose.Hello,     	Expression.Happy,     Shades.Up,   Eyes.Down,     true),
			new SpriteEntry(241,	Pose.SeeHere,		Expression.Happy,     Shades.Up,   Eyes.Down,     true),
			new SpriteEntry(242,	Pose.SeeHere,   	Expression.Happy,     Shades.Mid,  Eyes.Down,     true),
			new SpriteEntry(243,	Pose.AtEase,    	Expression.Happy,     Shades.Mid,  Eyes.Down,     true),
			new SpriteEntry(244,	Pose.AtEase,    	Expression.Happy,     Shades.Up,   Eyes.Down,     true),
			new SpriteEntry(245,	Pose.AtEase,    	Expression.Happy,     Shades.Up,   Eyes.Down,     false),
			new SpriteEntry(246,	Pose.AtEase,    	Expression.Happy,     Shades.Mid,  Eyes.Down,     false),
			new SpriteEntry(247,	Pose.LaptopTyping,  Expression.Happy,     Shades.Mid,  Eyes.Down,     false),
			new SpriteEntry(248,	Pose.LaptopTyping,  Expression.Happy,     Shades.Up,   Eyes.Down,     false),
			new SpriteEntry(249,	Pose.LaptopTyping,  Expression.Happy,     Shades.Up,   Eyes.Down,     true),
			new SpriteEntry(250,	Pose.LaptopTyping,  Expression.Happy,     Shades.Mid,  Eyes.Down,     true),
			new SpriteEntry(251,	Pose.LaptopWatching,Expression.Happy,     Shades.Mid,  Eyes.Down,     true),
			new SpriteEntry(252,	Pose.LaptopWatching,Expression.Happy,     Shades.Up,   Eyes.Down,     true),
			new SpriteEntry(253,	Pose.LaptopWatching,Expression.Happy,     Shades.Up,   Eyes.Down,     false),
			new SpriteEntry(254,	Pose.LaptopPointing,Expression.Happy,     Shades.Up,   Eyes.Down,     false),
			new SpriteEntry(255,	Pose.LaptopPointing,Expression.Happy,     Shades.Mid,  Eyes.Down,     false),
			new SpriteEntry(256,	Pose.LaptopPointing,Expression.Happy,     Shades.Mid,  Eyes.Down,     true),
			new SpriteEntry(257,	Pose.LaptopPointing,Expression.Happy,     Shades.Up,   Eyes.Down,     true),
			new SpriteEntry(258,	Pose.Ecstatic,		Expression.Happy,     Shades.Mid,  Eyes.Down,     true),
			new SpriteEntry(259,	Pose.Ecstatic,		Expression.Happy,     Shades.Up,   Eyes.Down,     true),
			new SpriteEntry(260,	Pose.Ecstatic,		Expression.Happy,     Shades.Up,   Eyes.Down,     false),
			new SpriteEntry(261,	Pose.Ecstatic,		Expression.Happy,     Shades.Mid,  Eyes.Down,     false),

			new SpriteEntry(262,	Pose.AtEase,     	Expression.Condescending,     Shades.Mid,    Eyes.Closed,     false),
			new SpriteEntry(263,	Pose.AtEase,     	Expression.Condescending,     Shades.Up,     Eyes.Closed,     false),
			new SpriteEntry(264,	Pose.AtEase,     	Expression.Condescending,     Shades.Up,     Eyes.Closed,     true),
			new SpriteEntry(265,	Pose.AtEase,     	Expression.Condescending,     Shades.Down,   Eyes.Closed,     true),
			new SpriteEntry(266,	Pose.AtEase,     	Expression.Condescending,     Shades.Down,   Eyes.Closed,     false),
			new SpriteEntry(267,	Pose.SeeHere,     	Expression.Condescending,     Shades.Down,   Eyes.Closed,     false),
			new SpriteEntry(268,	Pose.SeeHere,     	Expression.Condescending,     Shades.Mid,    Eyes.Closed,     false),
			new SpriteEntry(269,	Pose.SeeHere,     	Expression.Condescending,     Shades.Up,     Eyes.Closed,     false),
			new SpriteEntry(270,	Pose.SeeHere,     	Expression.Condescending,     Shades.Up,     Eyes.Closed,     true),
			new SpriteEntry(271,	Pose.SeeHere,     	Expression.Condescending,     Shades.Mid,    Eyes.Closed,     true),
			new SpriteEntry(272,	Pose.SeeHere,     	Expression.Condescending,     Shades.Down,   Eyes.Closed,     true),
			new SpriteEntry(273,	Pose.Hello,     	Expression.Condescending,     Shades.Down,   Eyes.Closed,     true),
			new SpriteEntry(274,	Pose.Hello,     	Expression.Condescending,     Shades.Mid,    Eyes.Closed,     true),
			new SpriteEntry(275,	Pose.Hello,     	Expression.Condescending,     Shades.Up,     Eyes.Closed,     true),
			new SpriteEntry(276,	Pose.Hello,     	Expression.Condescending,     Shades.Up,     Eyes.Closed,     false),
			new SpriteEntry(277,	Pose.Hello,     	Expression.Condescending,     Shades.Mid,    Eyes.Closed,     false),
			new SpriteEntry(278,	Pose.Hello,     	Expression.Condescending,     Shades.Up,     Eyes.Closed,     false),
			new SpriteEntry(279,	Pose.LaptopTyping,  Expression.Condescending,     Shades.Up,     Eyes.Closed,     false),
			new SpriteEntry(280,	Pose.LaptopTyping,  Expression.Condescending,     Shades.Mid,    Eyes.Closed,     false),
			new SpriteEntry(281,	Pose.LaptopTyping,  Expression.Condescending,     Shades.Down,   Eyes.Closed,     false),
			new SpriteEntry(282,	Pose.LaptopTyping,  Expression.Condescending,     Shades.Down,   Eyes.Closed,     true),
			new SpriteEntry(283,	Pose.LaptopTyping,  Expression.Condescending,     Shades.Mid,    Eyes.Closed,     true),
			new SpriteEntry(284,	Pose.LaptopTyping,  Expression.Condescending,     Shades.Up,     Eyes.Closed,     true),
			new SpriteEntry(285,	Pose.LaptopPointing,Expression.Condescending,     Shades.Up,     Eyes.Closed,     true),
			new SpriteEntry(286,	Pose.LaptopPointing,Expression.Condescending,     Shades.Mid,    Eyes.Closed,     true),
			new SpriteEntry(287,	Pose.LaptopPointing,Expression.Condescending,     Shades.Down,   Eyes.Closed,     true),
			new SpriteEntry(288,	Pose.LaptopPointing,Expression.Condescending,     Shades.Down,   Eyes.Closed,     false),
			new SpriteEntry(289,	Pose.LaptopPointing,Expression.Condescending,     Shades.Mid,    Eyes.Closed,     false),
			new SpriteEntry(290,	Pose.LaptopPointing,Expression.Condescending,     Shades.Up,     Eyes.Closed,     false),
			new SpriteEntry(291,	Pose.LaptopWatching,Expression.Condescending,     Shades.Up,     Eyes.Closed,     false),
			new SpriteEntry(292,	Pose.LaptopWatching,Expression.Condescending,     Shades.Mid,    Eyes.Closed,     false),
			new SpriteEntry(293,	Pose.LaptopWatching,Expression.Condescending,     Shades.Down,   Eyes.Closed,     false),
			new SpriteEntry(294,	Pose.LaptopWatching,Expression.Condescending,     Shades.Down,   Eyes.Closed,     true),
			new SpriteEntry(295,	Pose.LaptopWatching,Expression.Condescending,     Shades.Mid,    Eyes.Closed,     true),
			new SpriteEntry(296,	Pose.LaptopWatching,Expression.Condescending,     Shades.Up,     Eyes.Closed,     true)
		};
	}

	public enum Pose
	{AtEase, SeeHere, Hello, LaptopWatching, LaptopPointing, LaptopTyping, Maniacal, Cross, Ecstatic}
	public enum Expression
	{Neutral, Dumbfonded, Condescending, Happy, ManiacalLaugh}
	public enum Shades
	{Up, Mid, Down}
	public enum Eyes
	{Right, Left, Down, Up, Closed}

	[Serializable]
	public struct SpriteEntry
	{
		public SpriteEntry(int id, Pose pose, Expression expression, Shades shades, Eyes eyes, bool? talk)
		{
			this.id = id;
			this.pose = pose;
			this.expression = expression;
			this.shades = shades;
			this.eyes = eyes;
			this.talk = talk;
		}

		public int id;
		public Pose pose;
		public Expression expression;
		public Shades shades;
		public Eyes eyes;
		public bool? talk;
	}
}


/*
(note to self)
levels of selection, listed as hiarchy.
Most expressions exist only on Neutral pose.
Some expressions are unique to a pose.

pose:
	At-ease
	See-here
	Hello
	Laptop-watching
	Laptop-pointing
	Laptop-typing
	Maniacal 
	Cross
	Ecstatic

Expression
	Neutral
	Dumbfonded <- ignores Talk, Eyes
	Condescending <- ignores Talk
	Happy
	Maniacal-laugh <- ignores talk
	(dizzy) <- requested for comission

Shades
	up
	mid
	down <- ignores Eyes

Eyes
	left
	right
	up
	down

Talk
	on
	off

 */



/*
old hard code:

			//-------pose-------
			//---- At ease ----
			//------------------
			if(pose == Pose.AtEase)
			{
				if(expression == Expression.Neutral)
				{
					if(shades == Shades.Up)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)	
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Mid)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
				if(expression == Expression.Dumbfonded)
				{
					if(shades == Shades.Up)   {return 0;}
					if(shades == Shades.Mid)  {return 0;}
					if(shades == Shades.Down) {return 0;}
				}
				if(expression == Expression.Condescending)
				{
					if(shades == Shades.Up)
						{if(talk) return 0; else return 0;}
					if(shades == Shades.Mid)
						{if(talk) return 0; else return 0;}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
				if(expression == Expression.Happy)
				{
					if(shades == Shades.Up)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)	
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Mid)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
			}
			//-------pose-------
			//---- See here ----
			//------------------
			if(pose == Pose.SeeHere)
			{
				if(expression == Expression.Neutral)
				{
					if(shades == Shades.Up)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)	
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Mid)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
				if(expression == Expression.Dumbfonded)
				{
					if(shades == Shades.Up)   {return 0;}
					if(shades == Shades.Mid)  {return 0;}
					if(shades == Shades.Down) {return 0;}
				}
				if(expression == Expression.Condescending)
				{
					if(shades == Shades.Up)
						{if(talk) return 0; else return 0;}
					if(shades == Shades.Mid)
						{if(talk) return 0; else return 0;}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
				if(expression == Expression.Happy)
				{
					if(shades == Shades.Up)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)	
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Mid)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
			}
			//----------pose------------
			//---- Hello / Greeting ----
			//--------------------------
			if(pose == Pose.Hello)
			{
				if(expression == Expression.Neutral)
				{
					if(shades == Shades.Up)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)	
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Mid)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
				if(expression == Expression.Dumbfonded)
				{
					if(shades == Shades.Up)   {return 0;}
					if(shades == Shades.Mid)  {return 0;}
					if(shades == Shades.Down) {return 0;}
				}
				if(expression == Expression.Condescending)
				{
					if(shades == Shades.Up)
						{if(talk) return 0; else return 0;}
					if(shades == Shades.Mid)
						{if(talk) return 0; else return 0;}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
				if(expression == Expression.Happy)
				{
					if(shades == Shades.Up)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)	
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Mid)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
			}
			//----------pose------------
			//---- Laptop - Watching ----
			//--------------------------
			if(pose == Pose.LaptopWatching)
			{
				if(expression == Expression.Neutral)
				{
					if(shades == Shades.Up)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)	
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Mid)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
				if(expression == Expression.Dumbfonded)
				{
					if(shades == Shades.Up)   {return 0;}
					if(shades == Shades.Mid)  {return 0;}
					if(shades == Shades.Down) {return 0;}
				}
				if(expression == Expression.Condescending)
				{
					if(shades == Shades.Up)
						{if(talk) return 0; else return 0;}
					if(shades == Shades.Mid)
						{if(talk) return 0; else return 0;}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
				if(expression == Expression.Happy)
				{
					if(shades == Shades.Up)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)	
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Mid)
					{
						if(eyes == Eyes.Right)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Left)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Down)
							{if(talk) return 0; else return 0;}
						if(eyes == Eyes.Up)
							{if(talk) return 0; else return 0;}
					}
					if(shades == Shades.Down)
						{if(talk) return 0; else return 0;}
				}
			}


 */