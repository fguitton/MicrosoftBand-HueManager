using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Markup;

using Microsoft.Xaml.Interactivity;

namespace Roboworks.HueManager.Behaviors
{
    [ContentProperty(Name = "Actions")]
    public class StateTransitionTriggerBehavior : Behavior<Button>
    {

#region Dependency Properties

        public static readonly DependencyProperty ActionsProperty = 
            DependencyProperty.Register(
                "Actions",
                typeof(ActionCollection),
                typeof(StateTransitionTriggerBehavior),
                new PropertyMetadata(null)
            );

        public static readonly DependencyProperty StoryboardProperty =
            DependencyProperty.Register(
                "Storyboard",
                typeof(Storyboard),
                typeof(StateTransitionTriggerBehavior),
                new PropertyMetadata(
                    null, 
                    new PropertyChangedCallback(StateTransitionTriggerBehavior.Storyboard_Changed)
                )
            );

        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register(
                "TargetObject",
                typeof(FrameworkElement),
                typeof(StateTransitionTriggerBehavior),
                new PropertyMetadata(null)
            );

#endregion

#region Properties

        public Storyboard Storyboard
        {
            get
            {
                return (Storyboard)this.GetValue(StateTransitionTriggerBehavior.StoryboardProperty);
            }
            set
            {
                this.SetValue(StateTransitionTriggerBehavior.StoryboardProperty, value);
            }
        }

        public ActionCollection Actions
        {
            get
            {
                var actionCollection = (ActionCollection)
                    this.GetValue(StateTransitionTriggerBehavior.ActionsProperty);

                if (actionCollection == null)
                {
                    actionCollection = new ActionCollection();
                    this.SetValue(StateTransitionTriggerBehavior.ActionsProperty, actionCollection);
                }

                return actionCollection;
            }
        }

        public FrameworkElement TargetObject
        {
            get
            {
                return (FrameworkElement)this.GetValue(StateTransitionTriggerBehavior.TargetObjectProperty);
            }
            set
            {
                this.SetValue(StateTransitionTriggerBehavior.TargetObjectProperty, value);
            }
        }

        private string _stateName;
        public string StateName
        {
            get
            {
                return this._stateName;
            }
            set
            {
                this._stateName = value;
            }
        }

#endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Click += this.Button_Click;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Click -= this.Button_Click;

            base.OnDetaching();
        }

#region Private Static Methods
        private static void Storyboard_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (StateTransitionTriggerBehavior)d;
            behavior.StoryboardSubscribe((Storyboard)e.OldValue, (Storyboard)e.NewValue);
        }

#endregion

#region Private Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var control = this.TargetObject as Control;

            if (control != null && this.StateName != null)
            {
                VisualStateManager.GoToState(control, this.StateName, true);
            }
        }

        private void StoryboardSubscribe(Storyboard oldValue, Storyboard newValue)
        {
            if (oldValue != null)
            {
                this.Storyboard.Completed -= this.Storyboard_Completed;
            }

            if (newValue != null)
            {
                this.Storyboard.Completed += this.Storyboard_Completed;
            }
        }

        private void Storyboard_Completed(object sender, object e)
        {
            Interaction.ExecuteActions(this.TargetObject, this.Actions, null);
        }

#endregion

    }
}
