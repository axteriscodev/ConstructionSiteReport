using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Model.DocumentCompilation
{
    public class DocumentNavigator
    {
        public delegate void AnchorsChanged(object sender);
        public event AnchorsChanged OnAnchorsChanged;
        public List<DocumentAnchor> Anchors { get; set; } = [];

        public void AddAnchor(DocumentAnchor newAnchor)
        {
            // se l'ancora non è nella lista la aggiungo
            if (!Anchors.Exists(x => x.Anchor.Equals(newAnchor.Anchor)))
            {
                Anchors.Add(newAnchor);
                Anchors = Anchors.OrderBy(x => x.TypeIndex).ThenBy(x => x.Index).ToList();
                OnAnchorsChanged?.Invoke(this);
            }

        }





    }

}
