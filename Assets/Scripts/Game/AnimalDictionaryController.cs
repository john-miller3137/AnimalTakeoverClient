using System;
using Server.Game;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class AnimalDictionaryController : MonoBehaviour
    {
        public static AnimalDictionaryController Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            } 
        }

        [SerializeField] private GameObject nameHpDmg, abilities, portait;

        private TextMeshProUGUI nameHpDmgText, abilitiesText;

        private Image portaitImage;
        private void Start()
        {
            nameHpDmgText = nameHpDmg.GetComponent<TextMeshProUGUI>();
            abilitiesText = abilities.GetComponent<TextMeshProUGUI>();

            portaitImage = portait.GetComponent<Image>();
        }

        private String FormatNameHpDmg(string name, int hp, int dmg)
        {
            return "Name: " + name + "\n" + "\n" + "hp: " + hp + "\n" + "\n"
                   + "Damage: " + dmg;
        }

        public void SwitchToCat()
        {
            nameHpDmgText.text = FormatNameHpDmg("Cat", ConstantVars.cat_hp, ConstantVars.cat_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.cat_id];
            abilitiesText.text = "";
        }
        public void SwitchToBunny()
        {
            nameHpDmgText.text = FormatNameHpDmg("Bunny", ConstantVars.bunny_hp, ConstantVars.bunny_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.bunny_id];
            abilitiesText.text =
                "Passive Ability: No one knows where this bunny comes from but he loves leaving eggs" +
                ", with a 60% chance of doing so. These eggs contain either an extra move per turn, "
                + ConstantVars.clamshell_xp + " xp, or " + ConstantVars.chicken_egg_hp +
                " hp." + "\n" +
                "Special Ability: The bunny declares for all out war, using his own eggs as weapons of mass destruction. " +
                "Bunny deals x2 x(number of bunny eggs on map) damage.";
        }
        public void SwitchToChicken()
        {
            nameHpDmgText.text = FormatNameHpDmg("Chicken", ConstantVars.chicken_hp, ConstantVars.chicken_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.chicken_id];
            abilitiesText.text = "Passive Ability: Gotta lay eggs. Gotta lay eggs. Gotta lay eggs. This chicken has gotta lay eggs. The chicken has " +
                                 "a 50% chance of laying an egg when he moves. The eggs heal for " +
                                 ConstantVars.chicken_egg_hp +
                                 " hp." + "\n" +
                                 "Special Ability: The chicken launches his eggs in a firey storm dealing x2 x(number of chicken eggs on map) damage." +
                                 " e.g. 3 chicken eggs on map = x6 damage.";
        }
        public void SwitchToOwl()
        {
            nameHpDmgText.text = FormatNameHpDmg("Owl", ConstantVars.owl_hp, ConstantVars.owl_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.owl_id];
            abilitiesText.text = "Passive Ability: Has a 50% chance of picking up double seeds. \n\n\n" +
                                 "Special Ability: This naughty bird does x2 damage and steals its opponent's Xp.";
        }
        public void SwitchToSeal()
        {
            nameHpDmgText.text = FormatNameHpDmg("Seal", ConstantVars.seal_hp, ConstantVars.seal_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.seal_id];
            abilitiesText.text = "Passive Ability: The seal drops clam shells with a 50% chance on move. Clam shells provide " +
                                 ConstantVars.clamshell_xp + " xp on pickup."+ "\n\n"
                + "Special Ability: The seal traps its opponent in the clam shells. It does x2 x(number of clam shells).";
        }
        public void SwitchToWhale()
        {
            nameHpDmgText.text = FormatNameHpDmg("Whale", ConstantVars.whale_hp, ConstantVars.whale_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.whale_id];
            abilitiesText.text = "";
        }
        
        public void SwitchToDragon()
        {
            nameHpDmgText.text = FormatNameHpDmg("Dragon", ConstantVars.dragon_hp, ConstantVars.dragon_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.dragon_id];
            abilitiesText.text = "Passive Ability: Hot hot! You better collect the red fire spirit crystals." +
                                 " The Dragon deals x1.5 damage when using a fire attack. " + "\n" +"\n" +
                                 "Special Ability: Take cover. The dragon propels the hottest breath it can muster, " +
                                 " destroying 1 orb from every animal and burning to a crisp all plants on the map. Does x4 the damage";
        }
        
        public void SwitchToDeer()
        {
            nameHpDmgText.text = FormatNameHpDmg("Deer", ConstantVars.deer_hp, ConstantVars.deer_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.deer_id];
            abilitiesText.text = "Passive Ability: the deer can snack on plants, healing " + ConstantVars.deer_plant_health + " hp from each plant." +
                                 "\n\n" + "Special Ability: The deer does x2 damage and heals amount equal to damage.";
        }
        public void SwitchToDino()
        {
            nameHpDmgText.text = FormatNameHpDmg("Dino", ConstantVars.dino_hp, ConstantVars.dino_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.dino_id];
            abilitiesText.text = "Passive Ability: At the end of turns, the dino has a 20% chance of going extinct for 1 turn. While extinct," +
                                 " the animal acts as if its KOed. \n\n" + 
                                 "Special Ability: the dino calls upon the ancient spirits, summoning a giant firey meteor dealing x4 damage.";
        }
        public void SwitchToBear()
        {
            nameHpDmgText.text = FormatNameHpDmg("Mr. Bear", ConstantVars.bear_hp, ConstantVars.bear_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.bear_id];
            abilitiesText.text =
                "Passive Ability: Mr. Bear holds his teddy bear close and takes 20% less damage from attacks. " +
                "After being attacked, the teddy is dropped onto the board. " + "\n"
                + " Special Ability: Using his sophisticated style, mr. bear gets spiffy and becomes the ultimate bear. " + 
            "He takes 1/2 damage from attacks while in his tuxedo. Lasts 4 turns.";
        }
        public void SwitchToElephant()
        {
            nameHpDmgText.text = FormatNameHpDmg("Elephant", ConstantVars.elephant_hp, ConstantVars.elephant_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.elephant_id];
            abilitiesText.text = "Passive Ability: When attacking from a dirt tile, the elephant heals " + ConstantVars.elephant_dirt_health
                + " hp. \n\n" + "Special Ability: The elephant whips up a sandy attack, dealing x1.5 damage. If the elephant is standing " +
                "on dirt, the attack damages all opposing animals.";
        }
        public void SwitchToFrog()
        {
            nameHpDmgText.text = FormatNameHpDmg("Frog", ConstantVars.frog_hp, ConstantVars.frog_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.frog_id];
            abilitiesText.text = "";
        }
        public void SwitchToRhino()
        {
            nameHpDmgText.text = FormatNameHpDmg("Rhino", ConstantVars.rhino_hp, ConstantVars.rhino_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.rhino_id];
            abilitiesText.text = "";
        }
        public void SwitchToGoat()
        {
            nameHpDmgText.text = FormatNameHpDmg("Goat", ConstantVars.goat_hp, ConstantVars.goat_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.goat_id];
            abilitiesText.text =
                "Passive Ability: The goat loves to eat anything and everything: plants, crystals, and anything else you can think of. "
                + "He heals " + ConstantVars.goat_crystal_health + " hp for every crystal he munches on. " + "\n" +"\n" +"\n" +
                "Special Ability: The goat shows the world whose the GOAT. The Goat knocks out any animal he chooses.";
        }
        public void SwitchToHedgehog()
        {
            nameHpDmgText.text = FormatNameHpDmg("Hedgehog", ConstantVars.hedgehog_hp, ConstantVars.hedgehog_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.hedgehog_id];
            abilitiesText.text = "";
        }
        public void SwitchToHippo()
        {
            nameHpDmgText.text = FormatNameHpDmg("Hippo", ConstantVars.hippo_hp, ConstantVars.hippo_dmg);
            portaitImage.sprite = OpenCrateController.Instance.portraits[ConstantVars.hippo_id];
            abilitiesText.text = "";
        }
        
        
    }
}